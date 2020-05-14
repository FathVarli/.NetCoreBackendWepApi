using Core.Entities.Concrete;
using DataAccess.DataAccess;

namespace DataAccess.Abstract
{
    public interface IPasswordResetDal : IEntityRepository<ResetPassword>
    {
    }
}
