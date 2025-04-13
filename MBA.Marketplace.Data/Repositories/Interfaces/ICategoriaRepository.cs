using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA.Marketplace.Data.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> ListarAsync();
        Task<Categoria> CriarAsync(Categoria categoria);
        Task<Categoria> ObterPorIdAsync(Guid id);
        Task<bool> AtualizarAsync(Categoria categoria);
        Task RemoverAsync(Categoria categoria);
    }
}
