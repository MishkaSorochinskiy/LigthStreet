using System.ComponentModel.DataAnnotations;

namespace LigthStreet.AdminSite.Models
{
    public class RegisterModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
