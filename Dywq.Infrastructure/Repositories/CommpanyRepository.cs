using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.Repositories
{
    #region CommpanyRepository
    public class CommpanyRepository : Repository<Company, int, DomainContext>, ICommpanyRepository
    {
        public CommpanyRepository(DomainContext context) : base(context)
        {
        }
    }

    public interface ICommpanyRepository : IRepository<Company, int>
    { }
    #endregion
    #region CompanyUserRepository
    public class CompanyUserRepository : Repository<CompanyUser, int, DomainContext>, ICompanyUserRepository
    {
        public CompanyUserRepository(DomainContext context) : base(context)
        {
        }
    }

    public interface ICompanyUserRepository : IRepository<CompanyUser, int>
    { }
    #endregion
    #region CompanyFieldRepository
    public class CompanyFieldRepository : Repository<CompanyField, int, DomainContext>, ICompanyFieldRepository
    {
        public CompanyFieldRepository(DomainContext context) : base(context)
        {
        }
    }

    public interface ICompanyFieldRepository : IRepository<CompanyField, int>
    { }
    #endregion

    #region CompanyFieldDataRepository
    public class CompanyFieldDataRepository : Repository<CompanyFieldData, int, DomainContext>, ICompanyFieldDataRepository
    {
        public CompanyFieldDataRepository(DomainContext context) : base(context)
        {
        }
    }

    public interface ICompanyFieldDataRepository : IRepository<CompanyFieldData, int>
    { }
    #endregion

    #region CompanyFieldDefaultValueRepository
    public class CompanyFieldDefaultValueRepository : Repository<CompanyFieldDefaultValue, int, DomainContext>, ICompanyFieldDefaultValueRepository
    {
        public CompanyFieldDefaultValueRepository(DomainContext context) : base(context)
        {
        }
    }

    public interface ICompanyFieldDefaultValueRepository : IRepository<CompanyFieldDefaultValue, int>
    { }
    #endregion

    #region CompanyFieldGroupRepository
    public class CompanyFieldGroupRepository : Repository<CompanyFieldGroup, int, DomainContext>, ICompanyFieldGroupRepository
    {
        public CompanyFieldGroupRepository(DomainContext context) : base(context)
        {
        }
    }

    public interface ICompanyFieldGroupRepository : IRepository<CompanyFieldGroup, int>
    { }
    #endregion

}
