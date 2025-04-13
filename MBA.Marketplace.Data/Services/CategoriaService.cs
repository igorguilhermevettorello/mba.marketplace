using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Enums;
using MBA.Marketplace.Data.Repositories.Interfaces;
using MBA.Marketplace.Data.Services.Interfaces;

namespace MBA.Marketplace.Data.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IProdutoRepository _produtoRepository;
        public CategoriaService(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
        }
        public async Task<IEnumerable<Categoria>> ListarAsync()
        {
            return await _categoriaRepository.ListarAsync();
        }
        public async Task<Categoria> CriarAsync(CategoriaDto dto)
        {
            var categoria = await _categoriaRepository.CriarAsync(new Categoria
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                CreatedAt = DateTime.Now
            });
            return categoria;
        }
        public async Task<Categoria> ObterPorIdAsync(Guid id)
        {
            return await _categoriaRepository.ObterPorIdAsync(id);
        }
        public async Task<bool> AtualizarAsync(Guid id, CategoriaDto dto)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(id);
            if (categoria == null)
                return false;

            categoria.Nome = dto.Nome;
            categoria.Descricao = dto.Descricao;
            categoria.UpdatedAt = DateTime.Now;

            return await _categoriaRepository.AtualizarAsync(categoria);
        }
        public async Task<StatusRemocaoEnum> RemoverAsync(Guid id)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(id);
            if (categoria == null)
                return StatusRemocaoEnum.NaoEncontrado;

            var produto = await _produtoRepository.ListarPorCategoriaIdAsync(id, false);
            if (produto.Any())
                return StatusRemocaoEnum.VinculacaoProduto;

            await _categoriaRepository.RemoverAsync(categoria);
            return StatusRemocaoEnum.Removido;
        }
    }
}
