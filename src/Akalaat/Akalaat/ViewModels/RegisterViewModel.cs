using System.ComponentModel.DataAnnotations;

namespace Akalaat.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; } 

        [Required]
        public string Email { get; set; }
        

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
