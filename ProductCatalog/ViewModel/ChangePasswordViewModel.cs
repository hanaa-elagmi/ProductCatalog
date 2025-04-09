using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Not Matched.")]
        public string ConfirmPassword { get; set;}
        }
}
