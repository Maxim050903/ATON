using Core.Models;

namespace Api.Interfaces
{
    public interface IUserService
    {
        Task<string> DeleteUser(string LoginDelete, string LoginBy, int type);
        Task<List<User>> FindAllUsersByAge(int age);
        Task<List<User>> GetAllActiviteUsers();
        Task<User> GetUserByLogin(string login, string password);
        Task<Guid> RecoveryUser(Guid id);
        Task<User> UpdateUser(User user);
        Task<Guid> Register(Guid Id,
           string Login,
           string Password,
           string Name,
           int Gender,
           DateTime Birthday,
           bool isAdmin,
           string CreatedBy);
        Task<string> UpdatePassword(string login, string loginBy, string password);
        Task<string> UpdateLogin(string login, string loginBy,
            string NewLogin);
        Task<bool> GetLogins(string login);
    }
}