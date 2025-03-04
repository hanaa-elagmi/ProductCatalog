using Microsoft.AspNetCore.Identity;

namespace ProductCatalog.Models
{
	public class ApplicationUser:IdentityUser
	{
		public string Address { get; set; }
	}
}
