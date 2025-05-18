namespace ATON.DTOs
{
    public record UserCreateRequest
    (
        string Login,
        string Password,
        string Name,
        int Gender,
        DateTime Birthday,
        bool isAdmin
    );

    public record UserUpdateRequest
    (
        string Name,
        int Gender,
        DateTime Birthday
    );
}
