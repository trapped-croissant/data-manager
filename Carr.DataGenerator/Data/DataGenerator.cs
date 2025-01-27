using System.Diagnostics;
using System.Globalization;
using Carr.DataGenerator.Extensions;
using Carr.DataGenerator.Models;
using CsvHelper;

namespace Carr.DataGenerator.Data;

public interface IDataGenerator
{
    public Task<IResult> GenerateDataAsync(int columns, int rows);
}

public class DataGenerator : IDataGenerator
{
    public async Task<IResult> GenerateDataAsync(int columns, int rows)
    {
        var columnInfoList = await GenerateColumnInfo(columns);

        var timer = Stopwatch.StartNew();

        var processors = Environment.ProcessorCount;
        var countPerProcessor = rows / processors + 1;
        var dataTasks = new List<Task<List<dynamic>>>();
        var remainingRows = rows;

        for (var i = 0; i < processors; i++)
        {
            var rowCount = remainingRows < countPerProcessor ? remainingRows : countPerProcessor;

            if (rowCount <= 0) continue;
            
            dataTasks.Add(GenerateDynamicData(columnInfoList, rowCount));
            remainingRows -= countPerProcessor;
        }

        await Task.WhenAll(dataTasks);

        var data = new List<dynamic>();
        foreach (var dataTask in dataTasks)
        {
            data.AddRange(dataTask.Result);
        }

        timer.Stop();
        Console.WriteLine($"Threaded : Generated {rows} data in {timer.ElapsedMilliseconds} ms.");

        return await GenerateCsvFile(data);
    }

    private Task<List<ColumnInfo>> GenerateColumnInfo(int columns)
    {
        var columnData = new List<ColumnInfo>();

        var random = new Random();
        var dataTypeCount = Enum.GetNames<DataTypes>().Length;

        for (var i = 0; i < columns; i++)
        {
            var index = random.Next(0, dataTypeCount);
            var dataType = (DataTypes)index;

            switch (dataType)
            {
                case DataTypes.String:
                    columnData.Add(new ColumnInfo("Column" + (i + 1), dataType, random.Next(0, 1) == 0,
                        random.Next(-1, 500)));
                    break;
                case DataTypes.Boolean:
                case DataTypes.Integer:
                case DataTypes.Decimal:
                case DataTypes.DateTime:
                case DataTypes.Guid:
                    columnData.Add(new ColumnInfo("Column" + (i + 1), dataType, random.Next(0, 1) == 0, null));
                    break;
                default:
                    columnData.Add(new ColumnInfo("Column" + (i + 1), dataType, random.Next(0, 1) == 0,
                        random.Next(-1, 1_000)));
                    break;
            }
        }

        return Task.FromResult(columnData);
    }

    private Task<List<dynamic>> GenerateDynamicData(List<ColumnInfo> columns, int rows)
    {
        var stopwatch = Stopwatch.StartNew();

        return Task.Run(() =>
        {
            var dynamicData = new List<dynamic>();
            var random = new Random();

            for (var i = 0; i < rows; i++)
            {
                var properties = new Dictionary<string, dynamic>();
                foreach (var columnInfo in columns)
                {
                    switch (columnInfo.DataType)
                    {
                        case DataTypes.Integer:
                            properties.Add(columnInfo.Name, random.NextInt(int.MinValue, int.MaxValue));
                            break;
                        case DataTypes.String:
                            properties.Add(columnInfo.Name, random.NextString(columnInfo.Size ?? 500));
                            break;
                        case DataTypes.Decimal:
                            properties.Add(columnInfo.Name, random.NextDecimal(-12, 12));
                            break;
                        case DataTypes.Boolean:
                            properties.Add(columnInfo.Name, random.NextBool());
                            break;
                        case DataTypes.DateTime:
                            properties.Add(columnInfo.Name, random.NextDateTime(DateTime.MinValue, DateTime.Today));
                            break;
                        case DataTypes.Guid:
                            properties.Add(columnInfo.Name, Guid.NewGuid());
                            break;
                        default:
                            break;
                    }
                }

                dynamicData.Add(properties.ToDynamic());
            }

            stopwatch.Stop();
            Console.WriteLine($"Generated {rows} rows in {stopwatch.ElapsedMilliseconds} ms.");
            return dynamicData;
        });
    }

    private async Task<IResult> GenerateCsvFile(IEnumerable<dynamic> data)
    {
        //TODO :  run delete files async
        var currentDirectory = Directory.GetCurrentDirectory();
        var folderPath = Path.Combine(currentDirectory, "generated_files");
        Directory.CreateDirectory(folderPath);
        DeleteOldFiles(folderPath);
        var filePath = Path.Combine(folderPath, $"data_{DateTime.Now:yyyyMMddhhmmss}.csv");
        try
        {
            using var memoryStream = new MemoryStream();
            await using var writer = new StreamWriter(filePath);
            await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(data);

            return Results.File(filePath, contentType: "text/csv");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private void DeleteOldFiles(string path)
    {
        var files = Directory.GetFiles(path);

        foreach (var file in files)
        {
            var fi = new FileInfo(file);
            if (fi.LastAccessTime < DateTime.Now.AddMinutes(-5))
                fi.Delete();
        }
    }
}