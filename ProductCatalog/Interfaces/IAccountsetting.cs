using ProductCatalog.Models;
using ProductCatalog.ViewModel;

namespace ProductCatalog.Interfaces
{
	public interface IAccountsetting
	{
		public string GetCurrentUserId();

		public string ChangeName(string Name);

        public string ChangeEmail(string email);
		public Task<bool> ChangePassword(ProfileViewModel profileViewModel);
		public ApplicationUser ChangeProfilePhoto(string userId, string url);

        public ApplicationUser GetUserInfo();
	}
}
