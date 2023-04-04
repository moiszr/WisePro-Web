using System.ComponentModel.DataAnnotations;

namespace WisePro_Web.Models
{
    public class RegisterManager
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Terms { get; set; }
    }
}