using System;

namespace Dywq.Domain.Abstractions
{
    public interface IEntity
    {
        object[] GetKeys();

        DateTime CreatedTime { get; }

        DateTime? UpdatedTime { get; }

    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }



    }
}
