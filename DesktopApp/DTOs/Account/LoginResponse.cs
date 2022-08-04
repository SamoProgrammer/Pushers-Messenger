namespace DesktopApp.DTOs.Account
{
    internal class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public string Error { get; set; }
    }
}
