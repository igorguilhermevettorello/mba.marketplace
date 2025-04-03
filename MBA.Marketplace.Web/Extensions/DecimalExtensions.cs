using System.Globalization;

namespace MBA.Marketplace.Web.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToPtBrCurrency(this decimal? valor)
        {
            return valor?.ToString("C", new CultureInfo("pt-BR")) ?? "R$ 0,00";
        }
    }
}
