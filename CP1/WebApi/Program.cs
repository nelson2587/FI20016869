using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger en Desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Lista que almacena enteros y flotantes
var list = new List<object>();

// GET /  -> redirige a Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

// POST / -> devuelve la lista; si header xml=true, devuelve XML
app.MapPost("/", ([FromHeader(Name = "xml")] bool xml = false) =>
{
    if (!xml)
    {
        // JSON por defecto
        return Results.Ok(list);
    }

    // Construir un XML simple con los valores como texto
    // (XmlSerializer no maneja bien List<object>; por eso proyectamos a string)
    var wrapper = new RandomListXml { Items = list.Select(x => x?.ToString() ?? string.Empty).ToList() };

    var serializer = new XmlSerializer(typeof(RandomListXml));
    using var ms = new MemoryStream();
    serializer.Serialize(ms, wrapper);
    var xmlString = Encoding.UTF8.GetString(ms.ToArray());

    // Devolver como XML
    return Results.Text(xmlString, "application/xml", Encoding.UTF8);
});

// PUT / -> agrega 'quantity' números aleatorios del 'type' indicado (int|float)
//         valida quantity>0 y type válido; errores -> 400 con {"error": "..."}
app.MapPut("/", ([FromForm] int quantity, [FromForm] string type) =>
{
    // Validaciones
    if (quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    if (string.IsNullOrWhiteSpace(type))
        return Results.BadRequest(new { error = "'type' is required (int|float)" });

    var typeNorm = type.Trim().ToLowerInvariant();
    if (typeNorm != "int" && typeNorm != "float")
        return Results.BadRequest(new { error = "'type' must be one of: int, float" });

    var random = new Random();

    if (typeNorm == "int")
    {
        for (int i = 0; i < quantity; i++)
            list.Add(random.Next());
    }
    else // float
    {
        for (int i = 0; i < quantity; i++)
            list.Add(random.NextSingle());
    }

    // 200 OK (puedes devolver la lista o un resumen)
    return Results.Ok(new
    {
        added = quantity,
        type = typeNorm,
        total = list.Count
    });
}).DisableAntiforgery();

// DELETE / -> elimina 'quantity' elementos desde el inicio de la lista
//             valida quantity>0 y que haya suficientes elementos
app.MapDelete("/", ([FromForm] int quantity) =>
{
    if (quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    if (list.Count < quantity)
        return Results.BadRequest(new { error = "list does not contain enough elements to delete the requested 'quantity'" });

    // Eliminar los primeros 'quantity' elementos
    list.RemoveRange(0, quantity);

    return Results.Ok(new
    {
        deleted = quantity,
        remaining = list.Count
    });
}).DisableAntiforgery();

// PATCH / -> limpia la lista (sin parámetros)
app.MapPatch("/", () =>
{
    list.Clear();
    return Results.Ok(new { message = "list cleared", total = list.Count });
});

app.Run();

// DTO para serialización XML
[XmlRoot("RandomList")]
public class RandomListXml
{
    [XmlElement("Item")]
    public List<string> Items { get; set; } = new();
}
