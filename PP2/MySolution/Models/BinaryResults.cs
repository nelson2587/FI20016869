namespace MySolution.Models;

public class BinaryResults
{
    public NumberBases A { get; set; } = new();
    public NumberBases B { get; set; } = new();

    public NumberBases And { get; set; } = new();
    public NumberBases Or  { get; set; } = new();
    public NumberBases Xor { get; set; } = new();

    public NumberBases Sum { get; set; } = new();
    public NumberBases Product { get; set; } = new();
}
