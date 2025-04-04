using AutoMapper;
using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Enums;
using MBA.Marketplace.Core.Extensions;
using MBA.Marketplace.Data.Services.Interfaces;
using MBA.Marketplace.Web.Filters;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.Web.Controllers
{
    [Route("categoria")]
    [Autorizado]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProdutoController> _logger;
        private readonly IMapper _mapper;
        public CategoriaController(ICategoriaService categoriaService, IHttpClientFactory httpClientFactory, ILogger<ProdutoController> logger, IMapper mapper) 
        {
            _categoriaService = categoriaService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.ListarAsync();
            var model = _mapper.Map<List<CategoriaViewModel>>(categorias);
            return View(model);
        }
        [HttpGet("criar")]
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost("criar")]
        public async Task<IActionResult> Criar(CategoriaFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var _ = _categoriaService.CriarAsync(new CategoriaDto
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao
                });

                ViewBag.RegistroSucesso = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro desconhecido ao registrar.");
                ViewBag.RegistroErro = true;
            }

            return View(model);
        }
        [HttpGet("editar/{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null)
            {
                ViewBag.RegistroNaoEncontrado = true;
                return View();
            }

            var model = _mapper.Map< CategoriaFormViewModel>(categoria);
            return View(model);
        }
        [HttpPost("editar/{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id, CategoriaFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var response = await _categoriaService.AtualizarAsync(id, new CategoriaDto
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao
                });

                if (!response)
                {
                    ModelState.AddModelError("Nome", "Categoria não encontrada.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro desconhecido ao registrar.");
                ViewBag.RegistroErro = true;
            }

            return View(model);
        }
        [HttpDelete("deletar/{id:Guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var status = await _categoriaService.RemoverAsync(id);
            if (status == StatusRemocaoEnum.NaoEncontrado)
                return NotFound();

            if (status == StatusRemocaoEnum.VinculacaoProduto)
            {
                var mensagem = status.GetDescription();
                return Conflict(new { mensagem });
            }

            return Ok();
        }
    }
}
