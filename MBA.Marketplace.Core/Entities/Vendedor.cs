using MBA.Marketplace.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MBA.Marketplace.Core.Entities
{
    [Table("Vendedores")]
    public class Vendedor
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public string Email { get; set; }
        public string UsuarioId { get; set; } 
        public ApplicationUser Usuario { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
