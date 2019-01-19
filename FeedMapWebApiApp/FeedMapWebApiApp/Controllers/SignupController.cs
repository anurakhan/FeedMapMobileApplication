using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FeedMapBLL.Domain;
using FeedMapBLL.Services;
using FeedMapBLL.Services.Abstract;
using FeedMapDAL;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;
using FeedMapWebApiApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class SignupController : Controller
    {
        private TokenManagerSingleton _tokenManager;
        private IUserService _service;
        private ClientAuthConfigObj _config;
        private IMapper _mapper;

        public SignupController(IUserService service,
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

            bool isSuccess = _service.SignUp();
            if (!isSuccess) return BadRequest();

            return Ok();
        }
    }
}
