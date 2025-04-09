using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Interfaces;
using ProductCatalog.Models;
using ProductCatalog.ViewModel;

namespace ProductCatalog.Reposatories
{
    public class UserRepo:IUser
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAccountsetting accountsettingRepo;

        public UserRepo(ApplicationDbContext dbContext,IAccountsetting accountsettingRepo)
        {
            this.dbContext = dbContext;
            this.accountsettingRepo = accountsettingRepo;
        }

        public void ChangeRole(string userId)
        {
            var userRole = dbContext.UserRoles.FirstOrDefault(x => x.UserId == userId);

            if (userRole != null)
            {
                // احذف العلاقة القديمة
                dbContext.UserRoles.Remove(userRole);
                dbContext.SaveChanges();

                // حدد الدور الجديد
                string newRoleId = (userRole.RoleId == "1") ? "2" : "1";

                // أضف العلاقة الجديدة
                var newUserRole = new IdentityUserRole<string>
                {
                    UserId = userId,
                    RoleId = newRoleId
                };

                dbContext.UserRoles.Add(newUserRole);
                dbContext.SaveChanges();
            }
        }


        public async Task< List<UserViewModel>> GetUsers()
        {
            var currentUserId= accountsettingRepo.GetCurrentUserId();
            var users = from user in dbContext.Users
                         join userRole in dbContext.UserRoles on user.Id equals userRole.UserId
                         join role in dbContext.Roles on userRole.RoleId equals role.Id
                         select new UserViewModel()
                         {
                             UserId = user.Id,
                             UserName = user.FullName,
                             UserRole = role.Name,
                             UserImage=user.ProfileImg
                         };

            var list = await users.Where(x=>x.UserId!=currentUserId).ToListAsync();
            return list;
        }
    }
}
