namespace DataBase.Entities
{
    public class UserEntity
    {
        public Guid id { get; set; } = Guid.Empty;
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public bool Admin { get; set; }
        public DateTime CreateOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime ModofiedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime RevokerOn { get; set; }
        public string RevokedBy { get; set; } = string.Empty;
    }
}
