using MBA.Marketplace.Core.DTOs;
using System.Security.Claims;

namespace MBA.Marketplace.Data.Services.Interfaces
{
    public interface IContaService
    {
        Task<(bool Success, string Token, IEnumerable<string> Errors, string UserId)> LoginAsync(LoginDto dto);

        Task<RetornoRegistrarUsuarioDto> RegisterAsync(RegistrarUsuarioDto dto);
        
    }
}
