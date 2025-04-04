using MBA.Marketplace.Core.Configurations;
using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Core.Services.Interfaces;
using MBA.Marketplace.Data.Data;
using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MBA.Marketplace.Data.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAppEnvironment _env;
        private readonly AppSettings _settings;
        public ProdutoService(ApplicationDbContext context, IAppEnvironment env, IOptions<AppSettings> options)
        {
            _context = context;
            _env = env;
            _settings = options.Value;
        }
        public async Task<IEnumerable<Produto>> ListarAsync(Vendedor vendedor)
        {
            return await _context
                .Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.VendedorId == vendedor.Id)
                .ToListAsync();
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
        public async Task<Produto> ObterPorIdAsync(Guid id, Vendedor vendedor)
        {
            var produto = await _context.Produtos.Where(p => p.Id == id && p.VendedorId == vendedor.Id).FirstOrDefaultAsync();
            if (produto != null) 
                produto.Src = $"{_settings.Url}/api/produtos/imagem/{produto.Imagem}";
            
            return produto;
        }
        public async Task<bool> AtualizarAsync(Guid id, ProdutoEditDto dto, Vendedor vendedor, IFormFile? imagem)
        {
            var produto = await _context.Produtos.Where(p => p.Id == id && p.VendedorId == vendedor.Id).FirstOrDefaultAsync();
            if (produto == null)
                return false;

            if (imagem != null)
            {
                string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
                string caminhoPasta = Path.Combine(_env.WebRootPath, "images", "produtos");

                if (!Directory.Exists(caminhoPasta))
                    Directory.CreateDirectory(caminhoPasta);

                string caminhoArquivo = Path.Combine(caminhoPasta, nomeArquivo);

                using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    await imagem.CopyToAsync(stream);
                }

                produto.Imagem = nomeArquivo;
            }
            

            produto.Nome = dto.Nome;
            produto.Descricao = dto.Descricao;
            produto.Preco = (decimal)dto.Preco;
            produto.Estoque = (int)dto.Estoque;
            produto.CategoriaId = (Guid)dto.CategoriaId;
            produto.VendedorId = vendedor.Id;
            produto.UpdatedAt = DateTime.Now;

            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoverAsync(Guid id, Vendedor vendedor)
        {
            var produto = await _context.Produtos.Where(p => p.Id == id && p.VendedorId == vendedor.Id).FirstOrDefaultAsync();
            if (produto == null)
                return false;

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
