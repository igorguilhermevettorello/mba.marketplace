using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MBA.Marketplace.Core.Entities
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(255)]
        public string Descricao { get; set; }
        [Required]
        public string Imagem { get; set; }
        [Required]
        public decimal Preco { get; set; }
        [Required]
        public int Estoque { get; set; }
        [Required]
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        [Required]
        public Guid VendedorId { get; set; }
        public Vendedor Vendedor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [NotMapped]
        public string Src { get; set; }
    }
}
