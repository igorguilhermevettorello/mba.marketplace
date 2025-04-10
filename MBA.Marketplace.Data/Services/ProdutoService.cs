using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Data;
using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MBA.Marketplace.Data.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public ProdutoService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<Produto>> ListarAllAsync()
        {
            return await _context
                    .Produtos
                    .Include(p => p.Categoria)
                    .Include(p => p.Vendedor)
                    .ToListAsync();
        }
        public async Task<IEnumerable<Produto>> ListarProdutosPorCategoriaAsync(Guid categoriaId) 
        {
            return await _context
                    .Produtos
                    .Include(p => p.Categoria)
                    .Include(p => p.Vendedor)
                    .Where(p => p.CategoriaId == categoriaId)
                    .ToListAsync();
        }
        public async Task<IEnumerable<Produto>> ListarProdutosPorCategoriaOuNomeDescricaoAsync(Guid? categoriaId, string descricao)
        {
            var query = _context.Produtos.AsQueryable();

            if (categoriaId != null)
            {
                query = query.Where(p => p.CategoriaId == categoriaId);
            }

            if (descricao != null)
            {
                query = query.Where(p => p.Nome.Contains(descricao));
            }

            return await query.ToListAsync();
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
            string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(dto.Imagem.FileName);
            var pasta = _config["SharedFiles:ImagensPath"];
            string caminhoPasta = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, pasta);

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
            return await _context.Produtos.Where(p => p.Id == id && p.VendedorId == vendedor.Id).FirstOrDefaultAsync();
        }
        public async Task<Produto> PublicObterPorIdAsync(Guid id)
        {
            return await _context
                .Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> AtualizarAsync(Guid id, ProdutoEditDto dto, Vendedor vendedor, IFormFile? imagem)
        {
            var produto = await _context.Produtos.Where(p => p.Id == id && p.VendedorId == vendedor.Id).FirstOrDefaultAsync();
            if (produto == null)
                return false;

            if (imagem != null)
            {
                string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
                var pasta = _config["SharedFiles:ImagensPath"];
                string caminhoPasta = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, pasta);

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
