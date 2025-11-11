using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BooksConsole.Data;
using BooksConsole.Models;

namespace BooksConsole
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Rutas en la raíz del PROYECTO (no /bin)
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var dataDir = Path.Combine(projectRoot, "data");
            Directory.CreateDirectory(dataDir);
            var csvPath = Path.Combine(dataDir, "books.csv");

            using var db = new AppDbContext();
            await db.Database.EnsureCreatedAsync();

            bool isEmpty =
                !await db.Authors.AnyAsync() &&
                !await db.Titles.AnyAsync() &&
                !await db.Tags.AnyAsync() &&
                !await db.TitlesTags.AnyAsync();   // coincide con tu DbSet

            if (isEmpty)
            {
                Console.WriteLine("La base de datos está vacía, será llenada a partir de ./data/books.csv");
                Console.WriteLine("Procesando...\n");

                if (!File.Exists(csvPath))
                {
                    Console.WriteLine($"❌ No se encontró CSV en: {csvPath}");
                    return 1;
                }

                var lines = await File.ReadAllLinesAsync(csvPath, Encoding.UTF8);
                int total = 0;

                foreach (var line in lines.Skip(1)) // salta encabezado
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var cols = Helpers.SplitCsvLine(line);
                    if (cols.Length < 3) continue;

                    var authorName = cols[0].Trim().Trim('"');
                    var titleName  = cols[1].Trim().Trim('"');
                    var tagsPart   = cols[2].Trim();

                    if (string.IsNullOrWhiteSpace(authorName) || string.IsNullOrWhiteSpace(titleName))
                        continue;

                    // Autor (buscar o crear)
                    var author = await db.Authors.FirstOrDefaultAsync(a => a.AuthorName == authorName);
                    if (author is null)
                    {
                        author = new Author { AuthorName = authorName };
                        db.Authors.Add(author);
                        await db.SaveChangesAsync();
                    }

                    // Título
                    var title = new Title { TitleName = titleName, AuthorId = author.AuthorId };
                    db.Titles.Add(title);
                    await db.SaveChangesAsync();

                    // Tags
                    var tags = tagsPart.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    foreach (var tagName in tags)
                    {
                        var tag = await db.Tags.FirstOrDefaultAsync(t => t.TagName == tagName);
                        if (tag is null)
                        {
                            tag = new Tag { TagName = tagName };
                            db.Tags.Add(tag);
                            await db.SaveChangesAsync();
                        }

                        db.TitlesTags.Add(new TitleTag { TitleId = title.TitleId, TagId = tag.TagId });
                    }

                    total++;
                    if (total % 50 == 0) Console.WriteLine($"  Insertadas {total} filas...");
                }

                await db.SaveChangesAsync();
                Console.WriteLine($"\n✅ Carga completa. Filas procesadas: {total}");
                Console.WriteLine("Listo.\n");
            }
            else
            {
                Console.WriteLine("\nLa base de datos se está leyendo para crear los archivos TSV.");
                Console.WriteLine("Procesando...\n");

                // PASO 1: consulta traducible a SQL (materializamos en memoria)
                var materialized = await db.Titles
                    .Include(t => t.Author)
                    .Include(t => t.TitleTags).ThenInclude(tt => tt.Tag)
                    .AsNoTracking()
                    .Select(t => new
                    {
                        AuthorName = t.Author.AuthorName,
                        TitleName  = t.TitleName,
                        Tags       = t.TitleTags.Select(tt => tt.Tag.TagName)
                    })
                    .ToListAsync();

                // PASO 2: aplanado en memoria para manejar títulos sin tags
                var rows = materialized
                    .SelectMany(r =>
                        r.Tags.Any()
                            ? r.Tags.Select(tag => new { r.AuthorName, r.TitleName, TagName = tag })
                            : new[] { new { r.AuthorName, r.TitleName, TagName = "" } }
                    )
                    .ToList();

                static char FirstChar(string? s)
                {
                    if (string.IsNullOrWhiteSpace(s)) return '\0';
                    var span = s.AsSpan().TrimStart();
                    return span.Length > 0 ? span[0] : '\0';
                }

                // Orden descendente por primera letra: Autor > Título > Etiqueta
                var ordered = rows.OrderByDescending(r => FirstChar(r.AuthorName))
                                  .ThenByDescending(r => FirstChar(r.TitleName))
                                  .ThenByDescending(r => FirstChar(r.TagName))
                                  .ToList();

                // Agrupar por inicial del autor y escribir TSV
                var groups = ordered.GroupBy(r =>
                {
                    var ch = FirstChar(r.AuthorName);
                    return ch == '\0' ? '_' : char.ToUpperInvariant(ch);
                });

                foreach (var g in groups)
                {
                    var fileName = $"{g.Key}.tsv";
                    var fullPath = Path.Combine(dataDir, fileName);

                    using var sw = new StreamWriter(fullPath, false, new UTF8Encoding(false));
                    await sw.WriteLineAsync("AuthorName\tTitleName\tTagName");
                    foreach (var row in g)
                        await sw.WriteLineAsync($"{row.AuthorName}\t{row.TitleName}\t{row.TagName}");
                }

                Console.WriteLine("\n✅ Todos los archivos TSV se generaron correctamente en /data.");
                Console.WriteLine("Listo.");
            }

            return 0;
        }
    }

    internal static class Helpers
    {
        public static string[] SplitCsvLine(string line)
        {
            var parts = new System.Collections.Generic.List<string>();
            bool inQuotes = false;
            var current = new StringBuilder();

            foreach (char c in line)
            {
                if (c == '"') inQuotes = !inQuotes;
                else if (c == ',' && !inQuotes) { parts.Add(current.ToString()); current.Clear(); }
                else current.Append(c);
            }
            parts.Add(current.ToString());
            return parts.ToArray();
        }
    }
}
