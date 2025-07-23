using System.ComponentModel.DataAnnotations;

namespace HolaMundoWebAPI.Validaciones
{
    public class PrimeraLetraMayusculaAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var valueString = value.ToString();
            var primeraLetra = value.ToString()[0].ToString();
            if(primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayúscula!");
            }
            return ValidationResult.Success;
        }
    }
}
