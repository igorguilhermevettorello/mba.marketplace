using MBA.Marketplace.API.Services.Interfaces;
using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Data.Data;

namespace MBA.Marketplace.API.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProdutoService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<Produto> CriarAsync(ProdutoDto dto, Vendedor vendedor)
        {
            // Salvar imagem
            string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(dto.Imagem.FileName);
            string caminhoPasta = Path.Combine(_env.WebRootPath, "images", "produtos");

            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);

            string caminhoArquivo = Path.Combine(caminhoPasta, nomeArquivo);

            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                await dto.Imagem.CopyToAsync(stream);
            }

            // Criar produto
            var produto = new Produto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = (decimal)dto.Preco,
                Estoque = (int)dto.Estoque,
                CategoriaId = (Guid)dto.CategoriaId,
                VendedorId = vendedor.Id,
                Imagem = nomeArquivo
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }
    }
}
