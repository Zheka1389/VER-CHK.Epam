using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VER_CHK.Helpers;
using VER_CHK.Interfaces.Users;
using VER_CHK.Models.Users;
using VER_CHK.Services;

namespace VER_CHK.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(UserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings.Value));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            var user = await _userService.Authenticate(model.UserName, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                user.UserName,
                user.Email,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody]RegisterModel model)
        {
            var user = _mapper.Map<User>(model);

            try
            {
                
                return Ok(await _userService.Create(user, model.Password));
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll() =>
            await _userService.GetAll();

        [HttpGet("{userName}", Name = "GetUser")]
        public async Task<ActionResult<UserModel>> Get(string userName)
        {
            var user = await _userService.Get(userName);
            if (user == null)
                return BadRequest(new { message = "Username is incorrect or not in base" });

            var model = _mapper.Map<UserModel>(user);
            return model;
        }

        [HttpPut("{userName}")]
        public async Task<IActionResult> Update(string userName, [FromBody]UpdateModel model)
        {
            var currentUser = await _userService.Get(userName);

            if (currentUser == null)
            {
                return BadRequest(new { message = "Username not found" });
            }

            var user = _mapper.Map<User>(model);
            user.UserName = userName;

            try
            {
                await _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{userName}")]
        public async Task<IActionResult> Delete(string userName)
        {
            var user = await _userService.Get(userName);

            if (user == null)
            {
                return BadRequest(new { message = "Username not found" });
            }

            await _userService.Delete(userName);

            return Ok();
        }
    }
}
