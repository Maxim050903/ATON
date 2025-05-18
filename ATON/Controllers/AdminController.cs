using Api.Interfaces;
using ATON.DTOs;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATON.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public AdminController(IUserService userService,
                               IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [Route("/Register")]
        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] UserCreateRequest request)
        {
            if (await _userService.GetLogins(request.Login))
            {
                var jwtReader = new JwtReader(HttpContext);

                var id = Guid.NewGuid();

                var CreatedBy = jwtReader.TakeLogin();

                await _userService.Register(id, request.Login,
                    request.Password, request.Name,
                    request.Gender, request.Birthday,
                    request.isAdmin, CreatedBy);
                return Ok();
            }
            return BadRequest("Login find");
        }

        [Route("/UpdateUser")]
        [HttpPatch]
        public async Task<ActionResult> UpdateUser(string login, [FromBody] UserUpdateRequest request)
        {
            var jwtReader = new JwtReader(HttpContext);
            var AdminLogin = jwtReader.TakeLogin();
            var user = await _userService.GetUserByLogin(login, null);

            user.Name = request.Name;
            user.Gender = request.Gender;
            user.Birthday = request.Birthday;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = AdminLogin;

            await _userService.UpdateUser(user);

            return Ok(user);
        }

        [Route("/UpdatePassword")]
        [HttpPatch]
        public async Task<ActionResult> UpdatePassword(string password, string login)
        {
            var jwtReader = new JwtReader(HttpContext);
            var AdminLogin = jwtReader.TakeLogin();

            var result = await _userService.UpdatePassword(login, AdminLogin, password);

            return Ok("Password changed");
        }

        [Route("/UpdateLogin")]
        [HttpPatch]
        public async Task<ActionResult> UpdateLogin(string login, string newLogin)
        {
            var jwtReader = new JwtReader(HttpContext);
            var AdminLogin = jwtReader.TakeLogin();

            var result = await _userService.UpdateLogin(login, AdminLogin, newLogin);

            return Ok(result);
        }

        [Route("/GetUsers")]
        [HttpGet]
        public async Task<ActionResult> GetActiviteUser()
        {
            var users = await _userService.GetAllActiviteUsers();
            return Ok(users);
        }

        [Route("/GetUser")]
        [HttpGet]
        public async Task<ActionResult> GetUserByLogin(string login)
        {
            var logins = await _userService.GetLogins(login);
            if (!logins)
            {
                var user = await _userService.GetUserByLogin(login, null);
                var isActive = user.RevokerOn == DateTime.MinValue ? true : false;
                var response = new AnswerForLoginRequest(user.Name, user.Gender, user.Birthday, isActive);
                return Ok(response);
            } 
            return BadRequest("login not found");
        }

        [Route("/GetUserByAge")]
        [HttpGet]
        public async Task<ActionResult> GetUserBigAge(int age)
        {
            var users = await _userService.FindAllUsersByAge(age);
            var response = new List<AnswerForLoginRequest>();

            foreach (var user in users)
            {
                var isActive = user.RevokerOn == DateTime.MinValue ? true: false;
                var value = new AnswerForLoginRequest(user.Name,
                    user.Gender, user.Birthday, isActive);

                response.Add(value);
            }
            return Ok(response);
        }

        [Route("/HardDelete")]
        [HttpDelete]
        public async Task<ActionResult> DeleteUserHard(string login)
        {
            var jwtReader = new JwtReader(HttpContext);
            var LoginBy = jwtReader.TakeLogin();

            //0 is hard delete
            await _userService.DeleteUser(login, LoginBy, 0);

            return Ok("User delete");
        }

        [Route("/SoftDelete")]
        [HttpPatch]
        public async Task<ActionResult> DeleteUserSoft(string login)
        {
            var jwtReader = new JwtReader(HttpContext);
            var LoginBy = jwtReader.TakeLogin();

            await _userService.DeleteUser(login, LoginBy, 1);

            return Ok("user deleted soft");
        }

        [Route("/RecoveryUser")]
        [HttpPatch]
        public async Task<ActionResult> RecoveryUser(Guid id)
        {
            await _userService.RecoveryUser(id);

            return Ok("user recovery");
        }
    }
}
