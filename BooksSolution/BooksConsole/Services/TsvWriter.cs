using System.Text;

namespace BooksConsole.Services;

public static class TsvWriter
{
    public static void WritePerInitial(IEnumerable<(string AuthorName, string TitleName, string TagName)> rows, string dataDir)
    {
        var groups = rows.GroupBy(r =>
        {
            var first = r.AuthorName.Length > 0 ? char.ToUpperInvariant(r.AuthorName[0]) : '#';
            return char.IsLetter(first) ? first : '#';
        });

        foreach (var grp in groups)
        {
            var outPath = Path.Combine(dataDir, $"{grp.Key}.tsv");
            using var fs = new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(fs, new UTF8Encoding(false));

            writer.WriteLine("AuthorName\tTitleName\tTagName");

            foreach (var row in grp)
            {
                writer.Write(row.AuthorName);
                writer.Write('\t');
                writer.Write(row.TitleName);
                writer.Write('\t');
                writer.WriteLine(row.TagName);
            }
        }
    }

    public static IEnumerable<(string AuthorName, string TitleName, string TagName)> OrderForOutput(IEnumerable<(string AuthorName, string TitleName, string TagName)> input)
    {
        char FirstChar(string s) => s.Length > 0 ? char.ToUpperInvariant(s[0]) : '\0';

        return input
            .OrderByDescending(r => FirstChar(r.AuthorName))
            .ThenByDescending(r => FirstChar(r.TitleName))
            .ThenByDescending(r => FirstChar(r.TagName));
    }
}
