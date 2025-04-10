using System.ComponentModel.DataAnnotations;

namespace MBA.Marketplace.Data.DTOs
{
    public class CategoriaDto
    {
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O {0} precisa ter entre {2} e {1} caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A Descrição é obrigatória.")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "A Descrição precisa ter entre {2} e {1} caracteres")]
        public string Descricao { get; set; }
    }
}
