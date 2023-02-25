namespace WisePro_Web.Models
{
    public class RegisterManager
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Terms { get; set; }

        public RegisterManager()
        {
            Name = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
