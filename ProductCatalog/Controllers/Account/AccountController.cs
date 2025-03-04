using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;
using ProductCatalog.ViewModel;

namespace ProductCatalog.Controllers.Account
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController(UserManager<ApplicationUser>userManager,SignInManager<ApplicationUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
        public IActionResult Registeration()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Registeration(RegistrationViewModel newUser)
		{
			if (ModelState.IsValid) 
			{
				ApplicationUser user = new ApplicationUser();
				user.UserName = newUser.UserName;
				user.Email = newUser.Email;
				user.Address = newUser.Address; 
				user.PasswordHash = newUser.Password;
				IdentityResult result=await userManager.CreateAsync(user,newUser.Password);

				if (result.Succeeded) 
				{
				//create cookie
					await signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction("Index", "Home");
				}
				else {
					//some issue===>sent it to user
                    foreach (var item in result.Errors)
                    {
						ModelState.AddModelError("", item.Description);
                    }
                }
			}
			return View();
		}
	}
}
