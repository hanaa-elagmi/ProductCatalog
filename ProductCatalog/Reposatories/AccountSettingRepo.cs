using Microsoft.AspNetCore.Identity;
using ProductCatalog.Data;
using ProductCatalog.Interfaces;
using ProductCatalog.Models;
using ProductCatalog.ViewModel;
using System.Security.Claims;

namespace ProductCatalog.Reposatories
{
	public class AccountSettingRepo : IAccountsetting
	{
		private readonly ApplicationDbContext dbContext;
		private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountSettingRepo(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser>userManager,SignInManager<ApplicationUser> signInManager)
        {
			this.dbContext = dbContext;
			this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
		public string GetCurrentUserId()
		{
			var user = httpContextAccessor.HttpContext?.User;
			return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
		public string ChangeName(string Name)
		{
            string userId = GetCurrentUserId();

            var user=dbContext.Users.Find(userId);
			user.FullName = Name;
			dbContext.SaveChanges();

            return user.Id;
		}

        public string ChangeEmail(string email)
        {
            string userId = GetCurrentUserId();

            var user = dbContext.Users.Find(userId);
            user.Email = email;
            dbContext.SaveChanges();
            return user.Id;
        }

        public async Task<bool> ChangePassword(ProfileViewModel profileViewModel)
        {
            try
            {
                string userId = GetCurrentUserId();

                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return false;
                }

                var result = await userManager.ChangePasswordAsync(user, profileViewModel.OldPassword, profileViewModel.NewPassword);

                if (result.Succeeded)
                {
                    await signInManager.RefreshSignInAsync(user);
                    return true;
                }
                else
                {
                    // طباعة الأخطاء لتشخيص المشكلة
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Code} - {error.Description}");
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }


        public ApplicationUser GetUserInfo()
		{
            string userId = GetCurrentUserId();
            var User = dbContext.Users.Find(userId);
            if (User != null)
            {
                return User;
            }
            return null;
        }

        public ApplicationUser ChangeProfilePhoto(string userId,string url)
        {
            var User = dbContext.Users.Find(userId);
            User.ProfileImg = url;
            dbContext.SaveChanges();
            return User;

        }

    }
}
