namespace Core.Models
{
    public class User
    {
        public Guid id { get; set; } = Guid.Empty;
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public bool Admin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime RevokerOn { get; set; }
        public string RevokedBy { get; set; } = string.Empty;



        public User(Guid id, string Login, string PasswordHash, string Name,
        int Gender, DateTime Birthday, bool Admin, DateTime CreatedOn,
        string CreatedBy, DateTime ModofiedOn, string? ModifiedBy,
        DateTime RevokerOn, string? RevokedBy)
        {
            this.id = id;
            this.Login = Login;
            this.PasswordHash = PasswordHash;
            this.Name = Name;
            this.Gender = Gender;
            this.Birthday = Birthday;
            this.Admin = Admin;
            this.CreatedOn = CreatedOn;
            this.CreatedBy = CreatedBy;
            this.ModifiedOn = ModofiedOn;
            this.ModifiedBy = ModifiedBy;
            this.RevokerOn = RevokerOn;
            this.RevokedBy = RevokedBy;
        }

        public static (User user, string error) CreateUser(Guid id, string Login, string PasswordHash, string Name,
            int Gender, DateTime Birthday, bool Admin, DateTime CreatedOn,
            string CreatedBy, DateTime ModofiedOn, string ModifiedBy,
            DateTime RevokerOn, string RevokedBy)
        {
            var error = string.Empty;

            error = Name == string.Empty ? "name is null" : "none";

            if (error == "none")
            {
                var user = new User(id, Login, PasswordHash, Name,
                                Gender, Birthday, Admin, CreatedOn,
                                CreatedBy, ModofiedOn, ModifiedBy, 
                                RevokerOn,RevokedBy);
                return (user, error);
            }
            else
            {
                return (null, error);
            }
        }
    }
}
