using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Dtos.UserDto;

namespace Business.Concrete
{
    public class UserManager:IUserService
    {
        private IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<OperationClaim> GetClaim(User user)
        {
            return _userDal.GetClaims(user);
        }

        public IResult Add(User user)
        {
             _userDal.Add(user);
            return new SuccessResult("User added!");
        }

        public User GetByEmail(string email)
        {
          return  _userDal.Get(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _userDal.Get(u => u.Id == id);
        }
    }
}
