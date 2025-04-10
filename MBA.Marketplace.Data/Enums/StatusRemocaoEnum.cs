using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MBA.Marketplace.Data.Enums
{
    public enum StatusRemocaoEnum
    {
        [Display(Name = "Removido")]
        [Description("Removido com sucesso!")]
        Removido = 1,
        [Display(Name = "Não encontrado")]
        [Description("Categoria não foi encontrada!")]
        NaoEncontrado = 2,
        [Display(Name = "Vinculada com produtos")]
        [Description("Categoria não pode ser removida pois está vinculada a um produto!")]
        VinculacaoProduto = 3,
    }
}
