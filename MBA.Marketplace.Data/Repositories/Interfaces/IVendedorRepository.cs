using MBA.Marketplace.Data.Entities;

namespace MBA.Marketplace.Data.Repositories.Interfaces
{
    public interface IVendedorRepository
    {
        Task<bool> CriarAsync(Vendedor vendedor);
        Task<Vendedor?> ObterPorUsuarioIdAsync(string usuarioId);
    }
}
