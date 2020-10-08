using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.DataAccess.Context;
using DataAccess.DataAccess.Entityframework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User>, IUserDal
    {

        private readonly PostgresqlContext _dataContext;
        public EfUserDal(PostgresqlContext dataContext)
            : base(dataContext)
        {
            _dataContext = dataContext;
        }
        public List<OperationClaim> GetClaims(User user)
        {
            if (user != null)
            {

                var result = from operationClaim in _dataContext.OperationClaims
                    join userOperationClaim in _dataContext.UserOperationClaims
                        on operationClaim.Id equals userOperationClaim.OperationClaimId
                    where userOperationClaim.UserId == user.Id
                    select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();
            }

            return null;
        }
    }
}
