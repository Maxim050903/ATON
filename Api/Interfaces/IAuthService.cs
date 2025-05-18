namespace Api.Interfaces
{
    public interface IAuthService
    {
        Task<string> LogIn(string login, string password);
    }
}