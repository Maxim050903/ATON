using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure
{
    public class JwtReader
    {
        private readonly HttpContext _context;

        public JwtReader(HttpContext context)
        {
            _context = context;
        }

        public string TakeLogin()
        {
            string token = _context.Request.Cookies["token"];

            var handler = new JwtSecurityTokenHandler();

            var jwttoken = handler.ReadJwtToken(token);

            var login = jwttoken.Claims.First(x => x.Type == "login").Value;

            return login;
        }
    }
}
