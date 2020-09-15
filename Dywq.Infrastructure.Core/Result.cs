using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.Core
{
    public class Result
    {

        [JsonProperty("message")]
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }


        [JsonProperty("code")]
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }


        public static Result Success()
        {
            var result = new Result
            {
                Code = 0,
                Message = "success"
            };
            return result;
        }
        public static Result Success(string msg)
        {
            var result = new Result
            {
                Code = 0,
                Message = msg
            };
            return result;
        }
        public static Result Failure()
        {
            var result = new Result
            {
                Code = -1,
                Message = "failure"
            };
            return result;
        }
        public static Result Failure(string msg)
        {
            var result = new Result
            {
                Code = -1,
                Message = msg
            };
            return result;
        }
        public static Result Instance(string message, int code)
        {
            var result = new Result
            {
                Code = code,
                Message = message
            };
            return result;
        }



        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

    }


    /// <summary>
    /// 返回值
    /// </summary>
    public class Result<T> : Result
    {


        [JsonProperty("data")]
        /// <summary>
        /// 数据
        /// </summary>
        public virtual T Data { get; set; }

        public static Result<T> Success(T obj)
        {
            var result = new Result<T>();
            result.Code = 0;
            result.Message = "success";
            result.Data = obj;
            return result;
        }

        public static Result<T> Success(T obj, string message)
        {
            var result = new Result<T>();
            result.Code = 0;
            result.Message = message;
            result.Data = obj;
            return result;
        }


        public new static Result<T> Failure(string msg)
        {
            var result = new Result<T>();
            result.Code = -1;
            result.Message = msg;
            return result;
        }


        public static Result<T> Failure(T obj, string msg)
        {
            var result = new Result<T>();
            result.Code = -1;
            result.Message = msg;
            result.Data = obj;
            return result;
        }

        public static Result<T> Instance(T obj, string message, int code)
        {
            var result = new Result<T>();
            result.Code = code;
            result.Message = message;
            result.Data = obj;
            return result;
        }
    }




    public class PageResult<T> : Result<IEnumerable<T>>
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// 当前第几页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 一页数量
        /// </summary>
        public int PageSize { get; set; }


        public string Pager { get; set; }


        public static PageResult<T> Success(IEnumerable<T> obj, int total, int pageIndex, int pageSize, string linkUrl)
        {
            var result = new PageResult<T>()
            {
                Code = 0,
                Data = obj,
                Message = "success",
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Pager = Core.Pager.Create(pageSize, pageIndex, total, linkUrl, 8)
            };
            return result;
        }

    }



    public class PageData<T>
    {

        public PageData(IEnumerable<T> data, int total)
        {

            this.Data = data;
            this.Total = total;
        }

        public IEnumerable<T> Data { get; }
        public int Total { get; }
    }
}
