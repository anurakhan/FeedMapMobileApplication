using System;
using System.Threading.Tasks;
using FeedMapApp.Models;
using FeedMapApp.Models.Abstract;

namespace FeedMapApp.Services
{
    public class UserAuthService
    {
        private UserData _userData;
        private RestService _restService;

        public UserAuthService(UserData userData)
        {
            _restService = new RestService();
            UserAuthHeaderBuilder headerBuilder = new UserAuthHeaderBuilder(_restService);
            headerBuilder.BuildHeaders();
            _userData = userData;
        }

        public async Task<bool> SignUp()
        {
            RestReturnObj<UserToken> returnObj = 
                await _restService.UserAuthRequest("Signup", _userData);

            if (returnObj.IsSuccess) 
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Login()
        {
            
            var tokenPersistanceService = new TokenPersistanceService();

            RestReturnObj<UserToken> returnObj = 
                await _restService.UserAuthRequest("Login", _userData);
            
            if (returnObj.IsSuccess)
            {
                string token = returnObj.Obj.Token;
                bool success = tokenPersistanceService.SaveToken(token);
                return success;
            }
            return false;
        }

    }
}
