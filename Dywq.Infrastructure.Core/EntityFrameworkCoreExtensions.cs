﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dywq.Infrastructure.Core
{
    public static class EntityFrameworkCoreExtensions
    {
        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, params object[] parameters)
        {
            var conn = facade.GetDbConnection();
            connection = conn;
            //conn.Open();
            var cmd = conn.CreateCommand();

            cmd.Transaction = ((Microsoft.EntityFrameworkCore.Storage.RelationalTransaction)facade.CurrentTransaction).GetInfrastructure();
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(parameters);
            return cmd;
        }

        public static async Task<DataTable> SqlQueryAsync(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var reader = await command.ExecuteReaderAsync();
            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            //conn.Close();
            return dt;
        }

        public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            var dt = await SqlQueryAsync(facade, sql, parameters);
            return dt.ToList<T>();
        }

        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            var propertyInfos = typeof(T).GetProperties();
            var list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                var t = new T();
                foreach (PropertyInfo p in propertyInfos)
                {
                    if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                        p.SetValue(t, row[p.Name], null);
                }
                list.Add(t);
            }
            return list;
        }


        public static async Task<int> SqlCountAsync(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);

            var obj = await command.ExecuteScalarAsync();
            var count = Convert.ToInt32(obj);
            // conn.Close();
            return count;
        }



        public static async Task<int> SqlUpdateAsync(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var obj = await command.ExecuteNonQueryAsync();
            return obj;
        }


        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }

        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);


        public async static Task<PageData<T>> GetPageData<T>(this DatabaseFacade facade, string fields = "", string where = "", int pageIndex = 1, int pageSize = 10, string order = "id desc") where T : class, new()
        {
            var tableName = $"[{ typeof(T).Name}]";
            fields = string.IsNullOrWhiteSpace(fields) ? "*" : fields;
            var _where = !string.IsNullOrWhiteSpace(where) ? $"WHERE {where}" : "";
            var sql = $"select count(*) from {tableName} {_where}";

            Console.WriteLine($"count:{sql}");

            var count = await SqlCountAsync(facade, sql);
            if (count < 1) return new PageData<T>(null, 0);
            sql = $"SELECT {fields} FROM (SELECT  ROW_NUMBER() OVER ( ORDER BY {order}) as RowId, *  FROM      {tableName} {_where} ) AS result WHERE RowId > {(pageIndex - 1) * pageSize} AND RowId <= {(pageIndex * pageSize)} ORDER BY RowId";

            Console.WriteLine($"data:{sql}");
            var data = await SqlQueryAsync<T>(facade, sql);

            return new PageData<T>(data, count);


        }


    }
}
