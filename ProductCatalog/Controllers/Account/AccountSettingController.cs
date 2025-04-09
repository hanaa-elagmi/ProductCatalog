using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Interfaces;
using ProductCatalog.Models;
using ProductCatalog.Reposatories;
using ProductCatalog.ViewModel;
using System.Security.Claims;

namespace ProductCatalog.Controllers.Account
{
    public class AccountSettingController : Controller
    {
        private readonly IAccountsetting accountsettingRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountSettingController(IAccountsetting accountsettingRepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.accountsettingRepo = accountsettingRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var CurrentUser = accountsettingRepo.GetUserInfo();
            ProfileViewModel profileViewModel = new ProfileViewModel();
            profileViewModel.FullName = CurrentUser.FullName;
            profileViewModel.Email = CurrentUser.Email;
            profileViewModel.UserName = CurrentUser.UserName;
            profileViewModel.ProfileImgUrl = CurrentUser.ProfileImg; 

            return View(profileViewModel);
        }

        public IActionResult  ChangeName()
        {
            var user = accountsettingRepo.GetUserInfo();
            ProfileViewModel profileViewModel = new ProfileViewModel();
            profileViewModel.FullName=user.FullName;
            return View("ChangeName",profileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult>ChangeName(ProfileViewModel profileViewModel)
        {

            var userid =accountsettingRepo.ChangeName(profileViewModel.FullName);
            var user = await _userManager.FindByIdAsync(userid);
            var existingClaims = await _userManager.GetClaimsAsync(user);
            // Remove old FullName claim if it exists
            var fullNameClaim = existingClaims.FirstOrDefault(c => c.Type == "FullName");
            if (fullNameClaim != null)
            {
                await _userManager.RemoveClaimAsync(user, fullNameClaim);
            }

            // Add new FullName claim
            await _userManager.AddClaimAsync(user, new Claim("FullName", user.FullName));

            // Refresh the identity cookie
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangeEmail()
        {
            var user = accountsettingRepo.GetUserInfo();
            ProfileViewModel profileViewModel = new ProfileViewModel();
            profileViewModel.Email = user.Email;

            return View(profileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ProfileViewModel profileViewModel)
        {
            
            var userid = accountsettingRepo.ChangeEmail(profileViewModel.Email);

            var user = await _userManager.FindByIdAsync(userid);
            var existingClaims = await _userManager.GetClaimsAsync(user);
            // Remove old FullName claim if it exists
            var EmailClaim = existingClaims.FirstOrDefault(c => c.Type == "Email");
            if (EmailClaim != null)
            {
                await _userManager.RemoveClaimAsync(user, EmailClaim);
            }

            // Add new FullName claim
            await _userManager.AddClaimAsync(user, new Claim("Email", user.Email));

            // Refresh the identity cookie
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ChangePhoto(ProfileViewModel profileViewModel)
        {
            var userid = accountsettingRepo.GetCurrentUserId();

            var projectFolder = Directory.GetCurrentDirectory();
            var relativeImagesPath = Path.Combine("wwwroot", "Images");
            var fullImagesPath = Path.Combine(projectFolder, relativeImagesPath);
            var fileName = $"{Guid.NewGuid()}_{profileViewModel.ProfileImg.FileName}";
            var fullImagePath = Path.Combine(fullImagesPath, fileName);
            using (var stream = new FileStream(fullImagePath, FileMode.Create))
            {
                profileViewModel.ProfileImg.CopyTo(stream);
                stream.Flush();
            }
            var url = $"{Request.Scheme}://{Request.Host}/Images/{fileName}";
            var myprofileImg = accountsettingRepo.ChangeProfilePhoto(userid, url);

            return RedirectToAction(nameof(Index));
        }
    }
}

