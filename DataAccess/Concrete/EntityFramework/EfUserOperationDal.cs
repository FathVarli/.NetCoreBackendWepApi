using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.DataAccess.Context;
using DataAccess.DataAccess.Entityframework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserOperationDal : EfEntityRepositoryBase<UserOperationClaim>, IUserOperationClaimDal
    {
        private readonly PostgresqlContext _dataContext;

        public EfUserOperationDal(PostgresqlContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
