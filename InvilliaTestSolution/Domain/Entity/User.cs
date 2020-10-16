namespace Domain.Model.Entity
{
    public class User
    {
        public User()
        {
        }
        public User(string userName, UserType userType, string passwordHash)
        {
            UserName = userName;
            UserType = userType;
            PasswordHash = passwordHash;
        }

        public int UserId { get; protected set; }
        public string UserName { get; protected set; }
        public UserType UserType { get; protected set; }
        public string PasswordHash { get; protected set; }


        public void Update(string userName, UserType userType, string passwordHash)
        {
            UserName = userName;
            UserType = userType;
            PasswordHash = passwordHash;
        }
    }
}
