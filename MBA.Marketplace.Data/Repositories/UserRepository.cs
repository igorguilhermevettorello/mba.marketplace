using MBA.Marketplace.Data.Models;
using MBA.Marketplace.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MBA.Marketplace.Data.Repositories
{
    public class UserRepository : IUserRepository<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser usuario, string senha)
        {
            return await _userManager.CheckPasswordAsync(usuario, senha);
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
