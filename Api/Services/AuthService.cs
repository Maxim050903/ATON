using Api.Interfaces;
using Infrastructure;

namespace Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> LogIn(string login, string password)
        {
            var user = await _userRepository.GetUserByLogin(login);

            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if (result)
            {
                var token = _jwtProvider.GenerateToken(user);

                return token;
            }
            throw new Exception("Faild LogIn");
        }
    }
}
