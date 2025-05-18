using Api.Interfaces;
using Core.Models;
using DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AtonDBContext _context;

        private User CreateUserOnEntity(UserEntity userEntity)
        {
            var user = User.CreateUser(
                   userEntity.id,
                   userEntity.Login,
                   userEntity.PasswordHash,
                   userEntity.Name,
                   userEntity.Gender,
                   userEntity.Birthday,
                   userEntity.Admin,
                   userEntity.CreateOn,
                   userEntity.CreatedBy,
                   userEntity.ModofiedOn,
                   userEntity.ModifiedBy,
                   userEntity.RevokerOn,
                   userEntity.RevokedBy).user;
            return user;
        }

        public UserRepository(AtonDBContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(Guid id)
        {
            var userEntity = await _context.Users.Where(x => x.id == id).FirstOrDefaultAsync();
            if (userEntity != null)
            {
                var user = CreateUserOnEntity(userEntity);
                return user;
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        //Создание пользователя по логину, паролю, имени, полу и дате рождения + указание будет ли
        //пользователь админом(Доступно Админам)
        public async Task<Guid> CreateUser(User user)
        {
            var UserEntity = new UserEntity
            {
                id = user.id,
                Login = user.Login,
                PasswordHash = user.PasswordHash,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedBy = user.CreatedBy,
                CreateOn = user.CreatedOn,
                ModifiedBy = user.ModifiedBy,
                ModofiedOn = user.ModifiedOn,
                RevokerOn = user.RevokerOn,
                RevokedBy = user.RevokedBy
            };
            await _context.Users.AddAsync(UserEntity);
            await _context.SaveChangesAsync();

            return UserEntity.id;
        }

        public async Task<List<string>> GetLogins()
        {
            var userEntites = await _context.Users.AsNoTracking().ToListAsync();
            
            var logins = userEntites.Select(x => x.Login).ToList();

            return logins;
        }

        public async Task<string> ChangeLogin(string login,string newLogin, 
                                             string loginBy)
        {
            await _context.Users.Where(x => x.Login == login)
                .ExecuteUpdateAsync(u => u
                .SetProperty(b => b.Login, newLogin)
                .SetProperty(b => b.ModifiedBy, loginBy)
                .SetProperty(b => b.ModofiedOn, DateTime.UtcNow));

            await _context.SaveChangesAsync();

            return ("Login changed");
        }

        //Изменение пароля (Пароль может менять либо Администратор, либо лично пользователь, если
        //он активен(отсутствует RevokedOn))
        public async Task<string> ChangePassword(string login,string loginBy,string passwordHash)
        {
            await _context.Users
                .Where(x => x.Login == login)
                .ExecuteUpdateAsync(u => u
                .SetProperty(b => b.PasswordHash, passwordHash)
                .SetProperty(b => b.ModofiedOn, DateTime.UtcNow)
                .SetProperty(b => b.ModifiedBy, loginBy));
            
            await _context.SaveChangesAsync();

            return ("Password changed");
        }

        //Изменение имени, пола или даты рождения пользователя
        //(Может менять Администратор, либо лично пользователь, если он активен(отсутствует RevokedOn))
        public async Task<User> UpdateUser(User user)
        {
            await _context.Users
                .Where(x => x.id == user.id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(b => b.Name, user.Name)
                .SetProperty(b => b.Gender, user.Gender)
                .SetProperty(b => b.Birthday, user.Birthday)
                .SetProperty(b => b.ModifiedBy, user.ModifiedBy)
                .SetProperty(b => b.ModofiedOn,user.ModifiedOn));

            await _context.SaveChangesAsync();

            return user;
        }

        //Запрос списка всех активных (отсутствует RevokedOn) пользователей, 
        //список отсортирован по CreatedOn(Доступно Админам)
        public async Task<List<User>> GetAllActiviteUsers()
        {
            var userEntities = await _context.Users.Where(x => x.RevokerOn == DateTime.MinValue).AsNoTracking().ToListAsync();

            var users = userEntities.Select(x => CreateUserOnEntity(x)).ToList();

            return users;
        }

        //Запрос пользователя по логину, в списке долны быть имя, пол и дата рождения,
        //статус активный или нет(Доступно Админам)
        //Запрос пользователя по логину и паролю (Доступно только самому пользователю,
        //если он активен(отсутствует RevokedOn))
        public async Task<User> GetUserByLogin(string login)
        {
                var UserEntity = await _context.Users.Where(x => x.Login == login).FirstOrDefaultAsync();

                var user = CreateUserOnEntity(UserEntity);

                return user;
        }

        //Запрос всех пользователей старше определённого возраста (Доступно Админам)
        public async Task<List<User>> FindAllUserByAge(int age)
        {
            var userEntities = await _context.Users.Where(x => x.Birthday < DateTime.UtcNow.AddYears(-age))
                .AsNoTracking().ToListAsync();
            var users = userEntities.Select(x => User.CreateUser(
                    x.id,
                    x.Login,
                    x.PasswordHash,
                    x.Name,
                    x.Gender,
                    (DateTime)x.Birthday,
                    x.Admin,
                    (DateTime)x.CreateOn,
                    x.CreatedBy,
                    (DateTime)x.ModofiedOn,
                    x.ModifiedBy,
                    (DateTime)x.RevokerOn,
                    x.RevokedBy).user).ToList();

            return users;
        }

        //Удаление пользователя по логину полное или мягкое (При мягком удалении должна
        //происходить простановка RevokedOn и RevokedBy) (Доступно Админам)
        public async Task<string> DeleteUser(string LoginDelete, string LoginBy, int type)
        {
            if (type == 0)
            {
                await _context.Users.Where(x => x.Login == LoginDelete).ExecuteDeleteAsync();

                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.Users.Where(x => x.Login == LoginDelete)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.RevokerOn, b => DateTime.UtcNow)
                    .SetProperty(b => b.RevokedBy, b => LoginBy));
            }
            return LoginDelete;
        }

        //Восстановление пользователя - Очистка полей (RevokedOn, RevokedBy) (Доступно Админам)
        public async Task<Guid> RecoveryUser(Guid id)
        {
            await _context.Users
                .Where(x => x.id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.RevokerOn, b => DateTime.MinValue)
                .SetProperty(b => b.RevokedBy, b => string.Empty));
            await _context.SaveChangesAsync();

            return id;
        }
    }
}
