using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MySolution.Models;

public sealed class BinaryStringAttribute : ValidationAttribute
{
    public BinaryStringAttribute()
    {
        ErrorMessage = "El valor debe ser binario (0/1), longitud > 0, ≤ 8 y múltiplo de 2.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var s = value as string ?? string.Empty;

        if (string.IsNullOrWhiteSpace(s))
            return new ValidationResult("El string no puede estar vacío.");

        if (!Regex.IsMatch(s, "^[01]+$"))
            return new ValidationResult("Solo se permiten los caracteres 0 y 1.");

        if (s.Length > 8)
            return new ValidationResult("La longitud no puede exceder 8 (un byte).");

        if (s.Length % 2 != 0)
            return new ValidationResult("La longitud debe ser múltiplo de 2 (2,4,6,8).");

        return ValidationResult.Success;
    }
}
