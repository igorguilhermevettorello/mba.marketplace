using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Core.Enums;

namespace MBA.Marketplace.API.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> ListarAsync();
        Task<Categoria> CriarAsync(CategoriaDto dto);
        Task<Categoria> ObterPorIdAsync(Guid id);
        Task<bool> AtualizarAsync(Guid id, CategoriaDto dto);
        Task<StatusRemocaoEnum> RemoverAsync(Guid id);
    }
}
