using MBA.Marketplace.Core.DTOs;

namespace MBA.Marketplace.Core.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string Token, IEnumerable<string> Errors)> LoginAsync(LoginDto dto);
    }
}
