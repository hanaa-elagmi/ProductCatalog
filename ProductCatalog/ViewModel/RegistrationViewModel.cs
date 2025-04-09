using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.ViewModel
{
	public class RegistrationViewModel
	{
		[Required]
		public string FullName { get; set; }
		[Required]
		public string UserName { get; set; }
		[DataType(DataType.Password)]
		[Required]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		public string? Address { get; set; }

	}
}
