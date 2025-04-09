using Microsoft.AspNetCore.Identity;

namespace ProductCatalog.Models
{
	public class ApplicationUser:IdentityUser
	{
		public string FullName { get; set; }
		public string Address { get; set; }
		public string? ProfileImg {  get; set; }
	}
}
