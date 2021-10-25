using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Vue.Splash_API.Dtos.Validation
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string extensions)
        {
            _extensions = extensions.Split(",");
        }

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                return new ValidationResult("No file detected");
            }

            var extension = Path.GetExtension(file.FileName);
            return _extensions.Contains(extension.ToLower())
                ? ValidationResult.Success
                : new ValidationResult(GetErrorMessage());
        }

        private string GetErrorMessage()
        {
            return $"This extension is not allowed, allowed extensions are : {string.Join(",", _extensions)}";
        }
    }
}