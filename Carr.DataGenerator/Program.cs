using Carr.DataGenerator.Data;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddTransient<IDataGenerator, DataGenerator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/v1/generatedata",
        async (IDataGenerator dataGenerator, [FromQuery] int columns, [FromQuery] int records) =>
        await dataGenerator.GenerateDataAsync(columns, records))
    .WithName("GetGeneratedData");

app.Run();