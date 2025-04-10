﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MBA.Marketplace.Data.Attributes
{
    public class ImagemAttribute : ValidationAttribute
    {
        private readonly string[] _tiposPermitidos = new[]
        {
            "image/jpeg", "image/png", "image/gif", "image/webp"
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var arquivo = value as IFormFile;

            if (arquivo == null)
                return ValidationResult.Success;

            if (!_tiposPermitidos.Contains(arquivo.ContentType))
                return new ValidationResult("O arquivo precisa ser uma imagem válida (jpeg, png, gif, webp).");

            return ValidationResult.Success;
        }
    }
}
