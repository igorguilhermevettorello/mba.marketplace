using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace MBA.Marketplace.Data.Services.Interfaces
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
