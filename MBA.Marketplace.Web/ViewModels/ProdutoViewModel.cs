namespace MBA.Marketplace.Web.ViewModels
{
    public class ProdutoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal? Preco { get; set; }
        public int? Estoque { get; set; }
        public Guid? CategoriaId { get; set; }
        public string Categoria { get; set; }
        public string Imagem { get; set; } 
    }
}
