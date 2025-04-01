using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;

namespace MBA.Marketplace.API.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<Produto> CriarAsync(ProdutoDto dto, Vendedor vendedor);
    }
}
