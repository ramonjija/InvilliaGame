namespace InvilliaTest.DTO
{
    public class LoggedUserDto
    {
        public LoggedUserDto(string userName, string token)
        {
            UserName = userName;
            Token = token;
            TokenType = Security.Settings.TokenType;
        }

        public string UserName { get; set; }
        public string TokenType { get; set; }
        public string Token { get; set; }
    }
}
