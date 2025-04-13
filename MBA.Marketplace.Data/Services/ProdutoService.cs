using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Repositories.Interfaces;
using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MBA.Marketplace.Data.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IConfiguration _config;
        public ProdutoService(IProdutoRepository produtoRepository, IConfiguration config)
        {
            _produtoRepository = produtoRepository;
            _config = config;
        }
        public async Task<IEnumerable<Produto>> ListarAllAsync()
        {
            return await _produtoRepository.ListarAsync();
        }
        public async Task<IEnumerable<Produto>> ListarProdutosPorCategoriaAsync(Guid categoriaId) 
        {
            return await _produtoRepository.ListarPorCategoriaIdAsync(categoriaId, true);
        }
        public async Task<IEnumerable<Produto>> ListarProdutosPorCategoriaOuNomeDescricaoAsync(Guid? categoriaId, string descricao)
        {
            return await _produtoRepository.ListarProdutosPorCategoriaOuNomeDescricaoAsync(categoriaId, descricao);
        }
        public async Task<IEnumerable<Produto>> ListarAsync(Vendedor vendedor)
        {
            return await _produtoRepository.ListarPorVendedorIdAsync(vendedor);
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

            var produto = await _produtoRepository.CriarAsync(new Produto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = (decimal)dto.Preco,
                Estoque = (int)dto.Estoque,
                CategoriaId = (Guid)dto.CategoriaId,
                VendedorId = vendedor.Id,
                Imagem = nomeArquivo
            });

            return produto;
        }
        public async Task<Produto> ObterPorIdAsync(Guid id, Vendedor vendedor)
        {
            return await _produtoRepository.ObterPorIdPorVendedorIdAsync(id, vendedor);
        }
        public async Task<Produto> PublicObterPorIdAsync(Guid id)
        {
            return await _produtoRepository.ObterPorIdAsync(id);
        }
        public async Task<bool> AtualizarAsync(Guid id, ProdutoEditDto dto, Vendedor vendedor, IFormFile? imagem)
        {
            var produto = await _produtoRepository.ObterPorIdPorVendedorIdAsync(id, vendedor);
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
            return await _produtoRepository.AtualizarAsync(produto);
        }
        public async Task<bool> RemoverAsync(Guid id, Vendedor vendedor)
        {
            var produto = await _produtoRepository.ObterPorIdPorVendedorIdAsync(id, vendedor);
            if (produto == null)
                return false;

            return await _produtoRepository.RemoverAsync(produto);
        }
    }
}
