using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web_API.TokenService;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ITokenService _tokenService;

        public UsersController(DatabaseContext databaseContext, ITokenService tokenService)
        {
            this._databaseContext = databaseContext;
            this._tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<User> Users = _databaseContext.GetUsers();
            return Ok(Users);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] User UserModel)
        {
            List<User> Users = _databaseContext.GetUsers();
            if (Users.Where(a => a.UserName == UserModel.UserName).FirstOrDefault() != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserModel.UserName),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    expiration = accessToken.ValidTo,
                    user = UserModel.UserName
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> CreateUser([FromBody] User userModel)
        {
            if (_databaseContext.Users.Where(a => a.UserName == userModel.UserName).FirstOrDefault() == null)
            {
                _databaseContext.Users.Add(userModel);
                _databaseContext.SaveChanges();

                return StatusCode(200, "Created user successfully");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "user already exists");
        }

        [HttpPut]
        [Route("UpdateExistingUser")]
        public async Task<IActionResult> UpdateUser([FromBody] User userModel)
        {
            var user = _databaseContext.Users.Where(x => x.UserName == userModel.UserName).FirstOrDefault();
            if (user != null)
            {
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Email = userModel.Email;

                _databaseContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, "user updated successfully");
            }

            return StatusCode(StatusCodes.Status404NotFound, "user is not found to update");
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult>DeleteUser(int Id)
        {
            var user = _databaseContext.Users.Where(x => x.UserId == Id).FirstOrDefault();
            if (user != null)
            {
                _databaseContext.Users.Remove(user);
                _databaseContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, "user Deleted successfully");
            }

            return StatusCode(StatusCodes.Status404NotFound, "user is not found to delete");
        }
    }
}
