using System;
using FeedMapBLL.Domain;

namespace FeedMapBLL.Services.Abstract
{
    public interface IUserService
    {
        bool SignUp();

        bool Login();

        void Init(User user, int hashBytesNum, int saltBytesNum);
    }
}
