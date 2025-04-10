using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MBA.Marketplace.Data.Services
{
    public class ImagemService : IImagemService
    {
        private readonly IAppEnvironment _env;

        public ImagemService(IAppEnvironment env)
        {
            _env = env;
        }

        public void SalvarImagem(IFormFile imagem)
        {
            var pasta = Path.Combine(_env.WebRootPath, "images", "produtos");

            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            var caminho = Path.Combine(pasta, imagem.FileName);
            using var stream = new FileStream(caminho, FileMode.Create);
            imagem.CopyTo(stream);
        }
    }
}
