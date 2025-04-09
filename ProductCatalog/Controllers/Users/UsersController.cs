using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Interfaces;

namespace ProductCatalog.Controllers.Users
{
    [Authorize(Roles = "Admin")]

    public class UsersController : Controller
    {
        private readonly IUser userRepo;

        public UsersController(IUser UserRepo)
        {
            userRepo = UserRepo;
        }
        public async Task< IActionResult >Index()
        {
            var users= await userRepo.GetUsers();

            return View(users);
        }
        public IActionResult ChangeRole(string id)
        {
            userRepo.ChangeRole(id);
            return RedirectToAction(nameof(Index));
        }
       
    }
}
