using APP.Models;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Lottery API", Version = "v1" });
});
builder.Services.AddSingleton<LoteriaInfo>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Politica", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseCors("Politica");

//End Points


app.MapGet("/resultados", async (LoteriaInfo lottery, [FromQuery] int? page, [FromQuery] int? pageSize) =>
{
    int pageNumber = page ?? 1;  
    int itemsPerPage = pageSize ?? 10;

    if (pageNumber < 1 || itemsPerPage < 1)
        return Results.BadRequest("Los parámetros 'page' y 'pageSize' deben ser mayores a 0.");

    var resultados = await lottery.GetPaginatetDataAsync(pageNumber, itemsPerPage);

    return Results.Json(new
    {
        Total = resultados.Count,
        Page = pageNumber,
        pageSize = itemsPerPage,
        Data = resultados
    });
    
});


app.MapGet("/resultados/{id:int}", async (LoteriaInfo lottery, int Id) =>
{
    var resultados = await lottery.GetDataAsync();
    Loteria? resultado = resultados.FirstOrDefault(x => x.Id == Id);

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
