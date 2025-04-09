using ProductCatalog.Models;
using ProductCatalog.ViewModel;

namespace ProductCatalog.Interfaces
{
    public interface IUser
    {
        public Task<List<UserViewModel>> GetUsers();
        public void ChangeRole(string Userid);
    }
}
