using DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class AtonDBContext : DbContext
    {
        public AtonDBContext(DbContextOptions<AtonDBContext> options)
           : base(options)
        {
        }
        public DbSet<UserEntity> Users { get; set; }
    }
}
