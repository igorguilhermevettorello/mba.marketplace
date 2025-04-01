using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA.Marketplace.Core.DTOs
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
