using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Core.Utilities.Results;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<User> GetUsers();
        List<OperationClaim> GetClaim(User user);
        IResult Add(User user);
        User GetByEmail(string email);
        User GetById(int id);
    }
}
