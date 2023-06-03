using System.Net;
using Api.Interfaces;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;
        
        public UserController(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _cacheService = new CacheService(cache, _configuration.GetValue<int>("Cache:slidingExpirationInSeconds"), _configuration.GetValue<int>("Cache:absoluteExpirationInSeconds"));
        }
        
        [HttpGet()]
        [Route("getUserProfile")]
        public User GetUserProfile(string id)
        {
            var cachedUserProfile = _cacheService.GetData<User>(id);
            
            return cachedUserProfile;
        }

        [HttpPost()]
        [Route("postUserProfile")]
        public HttpStatusCode PostUserProfile(User userModel)
        {
            var flag = _cacheService.SetData(userModel.Id.ToString(), userModel);
            
            return flag ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }

    }
}
