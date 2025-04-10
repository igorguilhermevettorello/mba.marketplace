using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Enums;

namespace MBA.Marketplace.Data.Services.Interfaces
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
