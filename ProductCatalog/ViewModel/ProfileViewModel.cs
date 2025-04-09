using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.ViewModel
{
    public class ProfileViewModel
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public IFormFile? ProfileImg { get; set; }
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        [DataType(DataType.Password)]

        public string? NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string? ConfirmedNewPassword { get; set; }


        public string? ProfileImgUrl { get; set; }

    }
}
