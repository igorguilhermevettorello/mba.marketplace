using System.ComponentModel.DataAnnotations;

namespace MBA.Marketplace.Web.Attributes
{
    public class ValidarPrecoAttribute : ValidationAttribute
    {
        private readonly string _palavra;

        //public ValidarPrecoAttribute(string palavra)
        //{
        //    _palavra = palavra.ToLower();
        //}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var texto = value as string;
            if (!string.IsNullOrEmpty(texto) && texto.ToLower().Contains(_palavra))
            {
                return new ValidationResult($"O campo não pode conter a palavra '{_palavra}'.");
            }

            return ValidationResult.Success;
        }
    }
}
