using DevOne.Security.Cryptography.BCrypt;

namespace Core.Domain.Model.Users
{
    public class User : EntityBase<User>
    {
        public virtual string Email { get; set; }

        public virtual string Name { get; set; }

        public virtual string Password { get; set; }

        public virtual void HashPassword()
        {
            var salt = BCryptHelper.GenerateSalt(10);
            Password = BCryptHelper.HashPassword(Password, salt);
        }

        public virtual bool IsAuthenticated(string password)
        {
            return BCryptHelper.CheckPassword(password, Password);
        }
    }
}
