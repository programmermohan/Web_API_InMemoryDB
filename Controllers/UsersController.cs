using BusinessLayer;
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
    }
}
