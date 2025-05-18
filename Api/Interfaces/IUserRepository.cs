using Core.Models;

namespace Api.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> CreateUser(User user);
        Task<string> DeleteUser(string LoginDelete, string LoginBy, int type);
        Task<List<User>> FindAllUserByAge(int age);
        Task<List<User>> GetAllActiviteUsers();
        Task<User> GetUser(Guid id);
        Task<User> GetUserByLogin(string login);
        Task<Guid> RecoveryUser(Guid id);
        Task<User> UpdateUser(User user);
        Task<string> ChangePassword(string login, string loginBy, string passwordHash);
        Task<string> ChangeLogin(string login, string newLogin, string loginBy);
        Task<List<string>> GetLogins();
    }
}