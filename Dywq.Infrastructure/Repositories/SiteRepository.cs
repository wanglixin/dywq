using Dywq.Domain.SiteAggregate;
using Dywq.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.Repositories
{
    public class SiteRepository : Repository<SiteInfo, int, DomainContext>, ISiteRepository
    {
        public SiteRepository(DomainContext context) : base(context)
        {
        }
    }


    public interface ISiteRepository : IRepository<SiteInfo, int>
    { }

}
