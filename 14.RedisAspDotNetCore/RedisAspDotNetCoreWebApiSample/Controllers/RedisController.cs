using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace RedisAspDotNetCoreWebApiSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController:  ControllerBase
    {
        private readonly IRedisCacheClient _redis;

        public RedisController(IRedisCacheClient redis)
        {
            _redis = redis;
        }
         
        [HttpPost]
        public IActionResult Post(User fakeuser)
        {
            var user = new User()
            {
	            Firstname = "Taswar",
	            Lastname = "Bhatti",
	            Twitter = "@taswarbhatti",
	            Blog = "http://taswar.zeytinsoft.com"
            };

            bool added = _redis.Db0.Add("user:key", user, DateTimeOffset.Now.AddMinutes(10));

            if(added)
                return Ok();
            else
                return BadRequest("Cannot add user");
        }

        [HttpPost]
        [Route("db/create")]
        public IActionResult CreateSet()
        {
            //create using Redis Commands
            bool added = _redis.Db0.Database.SetAdd("setkey","1");
            if(added)
                return Ok();
            else
                return BadRequest("Cannot add user");
        }

        [HttpGet]
        [Route("db/getset")]
        public ActionResult<string> GetSet()
        {
            var setValues = _redis.Db0.Database.SetMembers("setkey");
                        
            return string.Join(",", setValues);
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = _redis.Db0.GetAll<User>(new string[] {"user:key"});
                        
            return users.Values.ToList();
        }

         // DELETE api/redis/5
        [HttpDelete("{key}")]
        public ActionResult Delete(string key)
        {
            var result = _redis.Db0.Remove(key);

            if(result)
                return Ok();
            else
                return BadRequest("Cannot delete user");
        }
    }

    public class User
    {
        public string Firstname { get; set;}
	    public string Lastname { get; set;}
	    public string Twitter { get; set;}
	    public string Blog { get; set;}
    }
}