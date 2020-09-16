using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.DataAccess.Entityframework;
using PostgresqlContext = DataAccess.DataAccess.Context.PostgresqlContext;

namespace DataAccess.Concrete.EntityFramework
{
   public class EfUserDal: EfEntityRepositoryBase<User,PostgresqlContext>,IUserDal
    {
        
        public List<OperationClaim> GetClaims(User user)
        {
            using var context = new PostgresqlContext();
         
            var result = from operationClaim in context.OperationClaims
                    join userOperationClaim in context.UserOperationClaims
                        on operationClaim.Id equals userOperationClaim.OperationClaimId
                    where userOperationClaim.UserId == user.Id
                    select new OperationClaim
                    {
                        Id = operationClaim.Id,
                        Name = operationClaim.Name
                    };

                return result.ToList();
            
        }
    }
}
