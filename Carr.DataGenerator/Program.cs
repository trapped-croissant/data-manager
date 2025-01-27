using Carr.DataGenerator.Data;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

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
        {
            if (columns == 0 || records == 0)
            {
                throw new Exception("Columns and rows are required.");
            }
            return await dataGenerator.GenerateDataAsync(columns, records);
        })
        .WithName("GetGeneratedData");

app.UseCors(corsBuilder => corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();