using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.DataAccess.Entityframework;
using PostgresqlContext = DataAccess.DataAccess.Context.PostgresqlContext;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPasswordResetDal : EfEntityRepositoryBase<ResetPassword>, IPasswordResetDal
    {
        public EfPasswordResetDal(PostgresqlContext context) : base(context)
        {
        }
    }
}
