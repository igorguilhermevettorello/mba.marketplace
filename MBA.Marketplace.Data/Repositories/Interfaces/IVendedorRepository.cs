using MBA.Marketplace.Data.Entities;

namespace MBA.Marketplace.Data.Repositories.Interfaces
{
    public interface IVendedorRepository
    {
        Task CriarAsync(Vendedor vendedor);
        Task<Vendedor?> ObterPorUsuarioIdAsync(string usuarioId);
    }
}
