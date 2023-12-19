using APP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Lottery API", Version = "v1" });
});
builder.Services.AddSingleton<LotteryInfo>();
var app = builder.Build();




//Get All Results
app.MapGet("/resultados", async (LotteryInfo lottery) =>
{
    var resultado = await lottery.GetDataAsync();
    return Results.Json(resultado);

});

//Get One Result With The Name Of Sorteo
app.MapGet("/resultados/{sorteo}", async (LotteryInfo lottery, string sorteo) =>
{
    var resultados = await lottery.GetDataAsync();
    var normalizedSorteo = RemoveDiacritics(sorteo);

    Data? resultado = resultados.FirstOrDefault(e =>
        RemoveDiacritics(e.Sorteo).Equals(normalizedSorteo, StringComparison.OrdinalIgnoreCase));
    if (resultado is null)
    return Results.NotFound();

    return Results.Ok(resultado);
});

static string RemoveDiacritics(string text)
{
    var normalizedString = text.Normalize(NormalizationForm.FormD);
    var stringBuilder = new StringBuilder();

    foreach (var c in normalizedString)
    {
        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
        if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            stringBuilder.Append(c);
    }

    return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lottery API V1");
});

app.Run();
