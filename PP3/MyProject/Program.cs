using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// ================== Swagger ==================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ================== Endpoint raíz ==================
app.MapGet("/", () => Results.Redirect("/swagger"))
   .WithName("RootRedirect");

// ================== Helpers ==================
static IResult Error(string message)
    => Results.Json(new { error = message }, statusCode: 400);

static bool ParseXmlHeader(bool? xmlHeader) => xmlHeader ?? false;

static int LetterCount(string token)
    => token.Count(char.IsLetter);

static string ToXml<T>(T obj)
{
    var serializer = new XmlSerializer(typeof(T));
    using var ms = new MemoryStream();
    serializer.Serialize(ms, obj);
    return Encoding.UTF8.GetString(ms.ToArray());
}

// ================== /include ==================
app.MapPost("/include/{position:int}",
    ([FromRoute] int position,
     [FromQuery] string? value,
     [FromForm] string? text,
     [FromHeader(Name = "xml")] bool? xml) =>
    {
        if (position < 0) return Error("'position' must be 0 or higher");
        if (string.IsNullOrWhiteSpace(value)) return Error("'value' cannot be empty");
        if (string.IsNullOrWhiteSpace(text)) return Error("'text' cannot be empty");

        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        if (position >= words.Count) words.Add(value!);
        else words.Insert(position, value!);

        var resultText = string.Join(' ', words);
        var dto = new ResultDto { Ori = text, New = resultText };

        var wantsXml = ParseXmlHeader(xml);
        if (wantsXml)
        {
            var xmlPayload = ToXml(dto);
            return Results.Bytes(Encoding.UTF8.GetBytes(xmlPayload), "application/xml");
        }

        return Results.Json(dto, statusCode: 200);
    })
   .DisableAntiforgery(); // ← Solución al error 500

// ================== /replace ==================
app.MapPut("/replace/{length:int}",
    ([FromRoute] int length,
     [FromQuery] string? value,
     [FromForm] string? text,
     [FromHeader(Name = "xml")] bool? xml) =>
    {
        if (length <= 0) return Error("'length' must be greater than 0");
        if (string.IsNullOrWhiteSpace(value)) return Error("'value' cannot be empty");
        if (string.IsNullOrWhiteSpace(text)) return Error("'text' cannot be empty");

        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var replaced = words.Select(w => LetterCount(w) == length ? value! : w);

        var resultText = string.Join(' ', replaced);
        var dto = new ResultDto { Ori = text, New = resultText };

        var wantsXml = ParseXmlHeader(xml);
        if (wantsXml)
        {
            var xmlPayload = ToXml(dto);
            return Results.Bytes(Encoding.UTF8.GetBytes(xmlPayload), "application/xml");
        }

        return Results.Json(dto, statusCode: 200);
    })
   .DisableAntiforgery(); // ← Solución al error 500

// ================== /erase ==================
app.MapDelete("/erase/{length:int}",
    ([FromRoute] int length,
     [FromForm] string? text,
     [FromHeader(Name = "xml")] bool? xml) =>
    {
        if (length <= 0) return Error("'length' must be greater than 0");
        if (string.IsNullOrWhiteSpace(text)) return Error("'text' cannot be empty");

        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var filtered = words.Where(w => LetterCount(w) != length);

        var resultText = string.Join(' ', filtered);
        var dto = new ResultDto { Ori = text, New = resultText };

        var wantsXml = ParseXmlHeader(xml);
        if (wantsXml)
        {
            var xmlPayload = ToXml(dto);
            return Results.Bytes(Encoding.UTF8.GetBytes(xmlPayload), "application/xml");
        }

        return Results.Json(dto, statusCode: 200);
    })
   .DisableAntiforgery(); // ← Solución al error 500

app.Run();

// ================== DTO ==================
[XmlRoot("Result")]
public sealed class ResultDto
{
    public string Ori { get; set; } = "";
    public string New { get; set; } = "";
}
