using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using DataAccess.DataAccess;

namespace DataAccess.Abstract
{
   public interface IRefreshTokenDal: IEntityRepository<RefreshToken>
    {
    }
}
