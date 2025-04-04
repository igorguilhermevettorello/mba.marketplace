using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA.Marketplace.Data.Services.Interfaces
{
    public interface IImagemService
    {
        void SalvarImagem(IFormFile imagem);
    }
}
