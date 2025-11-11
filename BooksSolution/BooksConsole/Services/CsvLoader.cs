using System.Text;

namespace BooksConsole.Services;

public static class CsvLoader
{
    public static IEnumerable<(string Author, string Title, string[] Tags)> ReadBooksCsv(string csvPath)
    {
        if (!File.Exists(csvPath))
            throw new FileNotFoundException($"No se encontró el archivo CSV en {csvPath}");

        using var reader = new StreamReader(csvPath, Encoding.UTF8, true);

        string? header = reader.ReadLine();
        if (header is null) yield break;

        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            var fields = ParseCsvLine(line);
            if (fields.Count < 3) continue;

            string author = fields[0].Trim();
            string title  = fields[1].Trim();
            string tagsRaw = fields[2].Trim();

            var tags = tagsRaw.Length == 0
                ? Array.Empty<string>()
                : tagsRaw.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            yield return (author, title, tags);
        }
    }

    private static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>();
        var sb = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    sb.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(c);
            }
        }

        result.Add(sb.ToString());
        return result;
    }
}
