using MBA.Marketplace.Core.DTOs;

namespace MBA.Marketplace.Data.Services.Interfaces
{
    public interface IContaService
    {
        Task<(bool Success, string Token, IEnumerable<string> Errors)> LoginAsync(LoginDto dto);

        Task<RetornoRegistrarUsuarioDto> RegisterAsync(RegistrarUsuarioDto dto);
        
    }
}
