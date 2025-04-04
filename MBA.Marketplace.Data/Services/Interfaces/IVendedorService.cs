using MBA.Marketplace.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA.Marketplace.Data.Services.Interfaces
{
    public interface IVendedorService
    {
        Task<Vendedor> ObterPorIdAsync(string id);
    }
}
