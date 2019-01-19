using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FeedMapBLL.Domain;
using FeedMapBLL.Services;
using FeedMapBLL.Services.Abstract;
using FeedMapDTO;
using FeedMapWebApiApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private TokenManagerSingleton _tokenManager;
        private IUserService _service;
        private ClientAuthConfigObj _config;
        private IMapper _mapper;

        public LoginController(IUserService service,
                               TokenManagerSingleton tokenManager,
                               IOptions<ClientAuthConfigObj> config,
                               IMapper mapper)
        {
            _service = service;
            _tokenManager = tokenManager;
            _config = config.Value;
            _mapper = mapper;
        }

        [ClientKeyAuthAttribute]
        [HttpPost]
        public IActionResult Post([FromBody]UserDataClient userData)
        {
            User user = _mapper.Map<User>(userData);
            _service.Init(user, Convert.ToInt32(_config.HashBytesNum),
                          Convert.ToInt32(_config.SaltBytesNum));

            if (_service.Login())
            {
                string token = _tokenManager.AddUserToPool(user.Id);
                return Ok(new TokenClient{ Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
