using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IMS.BE.Models
{
    public class Register
    {
        [Required]
        [Display(Name = "Name", Prompt = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "User Code", Prompt = "Kode User harus unik")]
        public string UserCode { get; set; } = string.Empty;

        [Required]
        public string UserRole { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", Prompt = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirmation Password tidak sama")]
        [Display(Name = "Password", Prompt = "Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
