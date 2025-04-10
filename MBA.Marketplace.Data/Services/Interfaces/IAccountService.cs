using MBA.Marketplace.Data.DTOs;

namespace MBA.Marketplace.Data.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string Token, IEnumerable<string> Errors)> LoginAsync(LoginDto dto);
    }
}
