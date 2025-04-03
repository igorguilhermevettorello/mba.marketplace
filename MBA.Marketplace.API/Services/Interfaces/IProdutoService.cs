using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;

namespace MBA.Marketplace.API.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> ListarAsync(Vendedor vendedor);
        Task<Produto> CriarAsync(ProdutoDto dto, Vendedor vendedor);
        Task<Produto> ObterPorIdAsync(Guid id, Vendedor vendedor);
        Task<bool> AtualizarAsync(Guid id, ProdutoEditDto dto, Vendedor vendedor, IFormFile? imagem);
        Task<bool> RemoverAsync(Guid id, Vendedor vendedor);
    }
}
