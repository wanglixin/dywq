﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dywq.Domain.Abstractions
{
    public abstract class Entity : IEntity
    {

        public Entity()
        {
            this.CreatedTime = DateTime.Now;
        }

        public abstract object[] GetKeys();
        public override string ToString()
        {
            return $"[Entity: {GetType().Name}] Keys = {string.Join(",", GetKeys())}";
        }

        #region 领取模型的事件添加和删除
        private List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedTime { get; protected set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime? UpdatedTime { get; protected set; }

        [Timestamp]
        public virtual byte[] Version { get; protected set; }

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }


        #endregion
    }



    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {


        int? _requestedHashCode;
        public virtual TKey Id { get; protected set; }
        public override object[] GetKeys()
        {
            return new object[] { Id };
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity<TKey> item = (Entity<TKey>)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id.Equals(this.Id);
        }


        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }



        //表示对象是否为全新创建的，未持久化的
        public bool IsTransient()
        {
           
            return EqualityComparer<TKey>.Default.Equals(Id, default);
        }

        public override string ToString()
        {
            return $"[Entity: {GetType().Name}] Id = {Id}";
        }


        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }
    }
}
