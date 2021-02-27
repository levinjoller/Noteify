namespace Noteify.Data.EntityFramework.Options
{
    public class SeedUserOptions
    {
        public const string NormalUser = "NormalUser";

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}