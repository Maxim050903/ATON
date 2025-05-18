using Api.Interfaces;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<Guid> Register(Guid Id, 
            string Login,
            string Password,
            string Name,
            int Gender,
            DateTime Birthday,
            bool isAdmin,
            string CreatedBy)
        {

            var hashedPassword = _passwordHasher.Generate(Password);

            var user = User.CreateUser(Id,Login,hashedPassword,Name,Gender,Birthday,isAdmin,DateTime.UtcNow,CreatedBy,DateTime.MinValue,string.Empty,DateTime.MinValue,string.Empty);

            if (user.error == "none")
            {
                return await _userRepository.CreateUser(user.user);
            }
            else
            {
                throw new Exception(user.error);
            }
        }

        public async Task<string> UpdatePassword(string login,string loginBy,string password)
        {
            var passwordHash = _passwordHasher.Generate(password);

            var result = await _userRepository.ChangePassword(login,loginBy,passwordHash);
            
            return result;
        }
        
        public async Task<string> UpdateLogin(string login,string  loginBy,
            string NewLogin)
        {
            var logins = await _userRepository.GetLogins();
            if (!logins.Contains(NewLogin))
            {
                await _userRepository.ChangeLogin(login,NewLogin,loginBy);
                return ("Login update");
            }
            else
            {
                throw new Exception("This login find");
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            return await _userRepository.UpdateUser(user);
        }

        public async Task<List<User>> GetAllActiviteUsers()
        {
            return await _userRepository.GetAllActiviteUsers();
        }

        public async Task<User> GetUserByLogin(string login, string password = null)
        {
            var user = await _userRepository.GetUserByLogin(login);
            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if (result)
            {
                return user;
            }
            throw new Exception("Not found");
        }

        public async Task<bool> GetLogins(string login)
        {
            var logins = await _userRepository.GetLogins();

            if (logins.Contains(login))
                return false;
            return true;
        }

        public async Task<List<User>> FindAllUsersByAge(int age)
        {
            return await _userRepository.FindAllUserByAge(age);
        }

        public async Task<string> DeleteUser(string LoginDelete, string LoginBy, int type)
        {
            return await _userRepository.DeleteUser(LoginDelete, LoginBy, type);
        }

        public async Task<Guid> RecoveryUser(Guid id)
        {
            return await _userRepository.RecoveryUser(id);
        }
    }
}
