using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModel;
using System.Security.Claims;

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
				user.FullName = newUser.FullName;
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
					var roles = await userManager.GetRolesAsync(userModel);

					//create cookie
					if (result.Succeeded)
					{

						// Get existing claims
						var claims = new List<Claim>
						{
                            new Claim(ClaimTypes.NameIdentifier, userModel.Id), // ✅ تخزين UserId
							new Claim(ClaimTypes.Name, userModel.UserName), // Standard username claim
							new Claim("FullName", userModel.FullName ?? "Unknown"),// Custom FullName claim
							new Claim(ClaimTypes.Email, userModel.Email)
							
						};
						foreach (var role in roles)
						{
							claims.Add(new Claim(ClaimTypes.Role, role));
						}

						var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
						var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        // Manually sign in with claims
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = loginUser.RememberMe, 
                            ExpiresUtc = loginUser.RememberMe ? DateTime.UtcNow.AddDays(14) : (DateTime?)null
                        };

                        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal, authProperties);


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
