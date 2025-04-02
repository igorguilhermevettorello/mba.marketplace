using System.ComponentModel.DataAnnotations;

namespace MBA.Marketplace.Web.ViewModels
{
    public class ProdutoFormViewModel
    {
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O {0} precisa ter entre {2} e {1} caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A Descrição é obrigatória.")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "A Descrição precisa ter entre {2} e {1} caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O Preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O Preço deve ser maior que zero.")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O {0} deve ser no mínimo 1.")]
        public int? Estoque { get; set; }

        [Required(ErrorMessage = "A Categoria é obrigatória.")]
        public Guid? CategoriaId { get; set; }

        [Required(ErrorMessage = "A Imagem é obrigatória.")]
        public IFormFile Imagem { get; set; } // Arquivo enviado
    }
}
