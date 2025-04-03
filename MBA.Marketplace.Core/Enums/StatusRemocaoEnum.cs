using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA.Marketplace.Core.Enums
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
