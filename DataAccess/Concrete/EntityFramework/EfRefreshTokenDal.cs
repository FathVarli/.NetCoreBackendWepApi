using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.DataAccess.Context;
using DataAccess.DataAccess.Entityframework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRefreshTokenDal: EfEntityRepositoryBase<RefreshToken>, IRefreshTokenDal
    {
        public EfRefreshTokenDal(PostgresqlContext context) : base(context)
        {
        }
    }
}
 