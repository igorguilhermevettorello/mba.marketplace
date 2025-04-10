using System.ComponentModel.DataAnnotations;

namespace MBA.Marketplace.Data.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public string Senha { get; set; }
    }
}
