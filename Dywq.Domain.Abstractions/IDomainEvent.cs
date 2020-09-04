using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Domain.Abstractions
{
    public interface IDomainEvent : INotification
    {
    }
}
