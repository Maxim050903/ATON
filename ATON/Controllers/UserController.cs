using Api.Interfaces;
using ATON.DTOs;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATON.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "NoRevoked")]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [Route("/UpdateUserByUser")]
        [HttpPatch]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateRequest request)
        {
            var jwtReader = new JwtReader(HttpContext);
            var Login = jwtReader.TakeLogin();
            var user = await _userService.GetUserByLogin(Login, null);

            user.Name = request.Name;
            user.Gender = request.Gender;
            user.Birthday = request.Birthday;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = Login;

            await _userService.UpdateUser(user);

            return Ok(user);
        }

        [Route("/UpdatePasswordByUser")]
        [HttpPatch]
        public async Task<ActionResult> UpdatePassword(string password)
        {
            var jwtReader = new JwtReader(HttpContext);
            var Login = jwtReader.TakeLogin();

            var result = await _userService.UpdatePassword(Login, Login, password);

            return Ok("password changed");
        }

        [Route("/UpdateLoginByUser")]
        [HttpPatch]
        public async Task<ActionResult> UpdateLogin(string newLogin)
        {
            var jwtReader = new JwtReader(HttpContext);
            var Login = jwtReader.TakeLogin();

            var result = await _userService.UpdateLogin(Login, Login, newLogin);

            return Ok(result);
        }

        [Route("/GetUserByLoginAndPassword")]
        [HttpGet]
        public async Task<ActionResult> GetUserByPassword(string login, string password)
        {
            var user = await _userService.GetUserByLogin(login, password);

            var response = new AnswerForLoginRequest(user.Name, user.Gender, user.Birthday, true);

            return Ok(response);
        }
    }
}
