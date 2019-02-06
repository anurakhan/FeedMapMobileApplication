using System;
using AutoMapper;
using FeedMapBLL.Domain;
using FeedMapBLL.Helpers;
using FeedMapBLL.Services.Abstract;
using FeedMapDAL;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;
using Microsoft.Extensions.Options;

namespace FeedMapBLL.Services
{
    public class UserService : IUserService
    {
        private User _user;
        private IUserDataRepository _repo;
        private IMapper _mapper;
        private int _hashBytesNum;
        private int _saltBytesNum;

        public UserService(RepositoryPayload repo,
                           IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo.GetUserDataRepository();
        }

        public void Init(User user,
                         int hashBytesNum,
                         int saltBytesNum)
        {
            _user = user;
            _hashBytesNum = hashBytesNum;
            _saltBytesNum = saltBytesNum;
        }

        private void CreateUser(User user)
        {
            var salt = EncryptionUtil.GenNewSalt(_saltBytesNum);
            user.GenPasswordHash(salt, _hashBytesNum);
        }

        public bool SignUp()
        {
            UserDTO userDto = _repo.GetUserByUserName(_user.UserName);
            if (userDto != null) return false;

            CreateUser(_user);

            _repo.Post(_mapper.Map<UserDTO>(_user));
            return true;
        }

        public bool Login()
        {
            UserDTO userDto = _repo.GetUserByUserName(_user.UserName);
            if (userDto == null) return false;

            User userFromRepo = _mapper.Map<User>(userDto);

            _user.GenPasswordHash(userFromRepo.PasswordSalt,
                                  _hashBytesNum);
            bool isEqual = _user.DatawiseEquals(userFromRepo);

            if (isEqual)
            {
                _user.Id = userFromRepo.Id;
                return true;
            }
            return false;
        }
    }
}
