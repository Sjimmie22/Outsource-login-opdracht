using System.ComponentModel.DataAnnotations;

namespace Login_Outsource_project.Models
{
    public class LoginViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
