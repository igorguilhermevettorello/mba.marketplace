using MBA.Marketplace.Core.DTOs;

namespace MBA.Marketplace.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string Token, IEnumerable<string> Errors)> LoginAsync(LoginDto dto);
    }
}
