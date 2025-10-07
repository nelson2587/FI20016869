using MySolution.Models;

namespace MySolution.Services;

public class BinaryService
{
    private static string Pad8(string s) => s.PadLeft(8, '0');
    private static int BinToInt(string bin) => Convert.ToInt32(bin, 2);

    private static NumberBases BasesFromInt(int value) => new()
    {
        Bin = Convert.ToString(value, 2),
        Oct = Convert.ToString(value, 8),
        Dec = value.ToString(),
        Hex = Convert.ToString(value, 16).ToUpper()
    };

    private static NumberBases BasesFromBinString(string bin)
    {
        int v = BinToInt(bin);
        return new NumberBases {
            Bin = bin,
            Oct = Convert.ToString(v, 8),
            Dec = v.ToString(),
            Hex = Convert.ToString(v, 16).ToUpper()
        };
    }

    private static string Bitwise(string a, string b, Func<char, char, char> op)
    {
        var len = Math.Max(a.Length, b.Length);
        a = a.PadLeft(len, '0');
        b = b.PadLeft(len, '0');

        var chars = new char[len];
        for (int i = 0; i < len; i++) chars[i] = op(a[i], b[i]);

        var res = new string(chars).TrimStart('0');
        return string.IsNullOrEmpty(res) ? "0" : res;
    }

    public BinaryResults Compute(string aRaw, string bRaw)
    {
        var a8 = Pad8(aRaw);
        var b8 = Pad8(bRaw);

        string andBin = Bitwise(a8, b8, (x, y) => (x == '1' && y == '1') ? '1' : '0');
        string orBin  = Bitwise(a8, b8, (x, y) => (x == '1' || y == '1') ? '1' : '0');
        string xorBin = Bitwise(a8, b8, (x, y) => (x != y) ? '1' : '0');

        int aVal = BinToInt(aRaw);
        int bVal = BinToInt(bRaw);

        return new BinaryResults {
            A = BasesFromBinString(a8),
            B = BasesFromBinString(b8),
            And = BasesFromBinString(andBin),
            Or  = BasesFromBinString(orBin),
            Xor = BasesFromBinString(xorBin),
            Sum = BasesFromInt(aVal + bVal),
            Product = BasesFromInt(aVal * bVal)
        };
    }
}
