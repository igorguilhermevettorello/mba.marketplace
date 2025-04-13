using MBA.Marketplace.Data.Models;

namespace MBA.Marketplace.Data.Repositories.Interfaces
{
    public interface IUserRepository<T>
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser usuario, string senha);
    }
}
