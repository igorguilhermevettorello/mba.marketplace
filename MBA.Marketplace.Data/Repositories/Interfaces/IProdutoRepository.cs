using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Enums;

namespace MBA.Marketplace.Data.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ListarPorCategoriaIdAsync(Guid categoriaId, bool include);
        Task<IEnumerable<Produto>> ListarProdutosPorCategoriaOuNomeDescricaoAsync(Guid? categoriaId, string descricao);
        Task<IEnumerable<Produto>> ListarPorVendedorIdAsync(Vendedor vendedor);
        Task<IEnumerable<Produto>> ListarAsync();
        Task<Produto> CriarAsync(Produto produto);
        Task<Produto> ObterPorIdPorVendedorIdAsync(Guid id, Vendedor vendedor);
        Task<Produto> ObterPorIdAsync(Guid id);
        Task<bool> AtualizarAsync(Produto produto);
        Task<bool> RemoverAsync(Produto produto);
    }
}
