using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using core.Services;

namespace core.Controllers
{
    using core.Models;

    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        private List<User> users;
        public UserController()
        {
            users = new List<User>
            {
                new User { UserId = 1, Name = "a", Password = "a", TaskManager = true},
                new User { UserId = 2, Name = "b", Password = "b"},
                new User { UserId = 3, Name = "Yocheved", Password = "yyy#"}
            };
        }
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User User)
        {
            var dt = DateTime.Now;


            var user = users.FirstOrDefault(u =>
                u.Name == User.Name
                && u.Password == User.Password
            );        

            if (user == null)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.TaskManager ? "TaskManager" : "Agent"),
                new Claim("userId", user.UserId.ToString()),
            };
            if (user.TaskManager)
                claims.Add(new Claim("UserType", "Agent"));

            var token = TaskTokenService.GetToken(claims);

            return new OkObjectResult(TaskTokenService.WriteToken(token));
        }
    }
}