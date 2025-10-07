using System.ComponentModel.DataAnnotations;

namespace MySolution.Models;

public class BinaryInput
{
    [Display(Name = "a")]
    [BinaryString(ErrorMessage = "a inválido: solo 0/1, >0, ≤8 y múltiplo de 2.")]
    public string A { get; set; } = "";

    [Display(Name = "b")]
    [BinaryString(ErrorMessage = "b inválido: solo 0/1, >0, ≤8 y múltiplo de 2.")]
    public string B { get; set; } = "";
}
