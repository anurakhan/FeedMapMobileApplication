using System;
using System.Collections.Generic;
using FeedMapDTO;

namespace FeedMapDAL.Repository.Abstract
{
    public interface IUserDataRepository
    {
        IEnumerable<UserDTO> GetUsers();
        UserDTO GetUser(int id);
        UserDTO GetUserByUserName(string userName);
        int Post(UserDTO user);
        void Update(UserDTO user, int id);
    }
}
