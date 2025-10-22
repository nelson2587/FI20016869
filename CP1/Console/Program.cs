using System;
using System.Collections.Generic;

public class Numbers
{
    private static readonly double N = 25;

    public static double Formula(double z)
    {
        return Round((z + Math.Sqrt(4 + Math.Pow(z, 2))) / 2);
    }

    public static double Recursive(double z)
    {
        return Round(Recursive(z, N) / Recursive(z, N - 1));
    }

    public static double Iterative(double z)
    {
        return Round(Iterative(z, N) / Iterative(z, N - 1));
    }

    // ---------- MEJORA: Memoización para la versión recursiva con chatgpt----------
    private static readonly Dictionary<(int n, double z), double> _memo = new();

    // f(z, n):
    //  - f(z, 0) = 1
    //  - f(z, 1) = 1
    //  - f(z, n) = z * f(z, n - 1) + f(z, n - 2), para n >= 2
    private static double Recursive(double z, double n)
    {
        int k = (int)n;
        var key = (k, z);

        if (_memo.TryGetValue(key, out var cached))
            return cached;

        double result;
        if (k <= 1)
        {
            result = 1.0;
        }
        else
        {
            result = z * Recursive(z, k - 1) + Recursive(z, k - 2);
        }

        _memo[key] = result;
        return result;
    }

    // ---------- ACTUALIZACIÓN: versión iterativa con chatgpt----------
    // Mismo f(z, n) calculado con bucle
    private static double Iterative(double z, double n)
    {
        int k = (int)n;
        if (k <= 1) return 1.0;

        double f0 = 1.0; // f(z, 0)
        double f1 = 1.0; // f(z, 1)

        for (int i = 2; i <= k; i++)
        {
            double next = z * f1 + f0; // f(z, i) = z * f(z, i-1) + f(z, i-2)
            f0 = f1;
            f1 = next;
        }
        return f1; // f(z, k)
    }

    private static double Round(double value)
    {
        return Math.Round(value, 10);
    }

    public static void Main(String[] args)
    {
        // Si usas C# 11 o anterior, reemplaza por: string[] metallics = new[] { ... };
        String[] metallics = [
            "Platinum", // [0]
            "Golden",   // [1]
            "Silver",   // [2]
            "Bronze",   // [3]
            "Copper",   // [4]
            "Nickel",   // [5]
            "Aluminum", // [6]
            "Iron",     // [7]
            "Tin",      // [8]
            "Lead",     // [9]
        ];

        for (var z = 0; z < metallics.Length; z++)
        {
            Console.WriteLine("\n[" + z + "] " + metallics[z]);
            Console.WriteLine(" ↳ formula(" + z + ")   ≈ " + Formula(z));
            Console.WriteLine(" ↳ recursive(" + z + ") ≈ " + Recursive(z));
            Console.WriteLine(" ↳ iterative(" + z + ") ≈ " + Iterative(z));
        }
    }
}
