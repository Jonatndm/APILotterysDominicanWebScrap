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


app.MapGet("/resultados", async (LotteryInfo lottery) =>
{
    var resultado = await lottery.GetDataAsync();
    return Results.Json(resultado);

});


app.MapGet("/resultados/{id:int}", async (LotteryInfo lottery, int Id) =>
{
    var resultados = await lottery.GetDataAsync();
    Data? resultado = resultados.FirstOrDefault(x => x.Id == Id);

    if (resultado is null)
        return Results.NotFound();

    return Results.Ok(resultado);
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lottery API V1");
});

app.Run();
