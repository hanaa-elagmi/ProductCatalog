using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModel;

namespace ProductCatalog.Controllers.Account
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;

        public AccountController(UserManager<ApplicationUser>userManager,SignInManager<ApplicationUser> signInManager,ApplicationDbContext context)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
            this.context = context;
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
				user.PhoneNumber = newUser.PhoneNumber;
				user.PasswordHash = newUser.Password;
				
				IdentityResult result=await userManager.CreateAsync(user,newUser.Password);

				if (result.Succeeded) 
				{
					//assign the first user as admin and then users
					await userManager.AddToRoleAsync(user, "User");
				//create cookie
					await signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction("Login", "Account");
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

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
			if (ModelState.IsValid)
			{
                ApplicationUser userModel = await userManager.FindByNameAsync(loginUser.UserName);
                if (userModel != null)
                {
                    //check if password is true
                    var result = await signInManager.PasswordSignInAsync(userModel, loginUser.Password, loginUser.RememberMe, false);
                    //create cookie
                    if (result.Succeeded)
                    {
                        //create cookie
                        await signInManager.SignInAsync(userModel, loginUser.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "UserName or Password is wrong");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "wrong data !!!");
                }
            }
            
			return View(loginUser);
        }

     
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
