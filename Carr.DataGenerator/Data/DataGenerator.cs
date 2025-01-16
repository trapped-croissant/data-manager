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
        var data = await GenerateDynamicData(columnInfoList, rows);

        using var memoryStream = new MemoryStream();
        await using var writer = new StreamWriter(memoryStream);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        await csv.WriteRecordsAsync(data);

        return Results.File(memoryStream.ToArray(), contentType: "text/csv");
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
                        properties.Add(columnInfo.Name, 123.45);
                        break;
                    case DataTypes.Boolean:
                        properties.Add(columnInfo.Name, random.NextBool());
                        break;
                    case DataTypes.DateTime:
                        properties.Add(columnInfo.Name, new DateTime(1970, 1, 1));
                        break;
                    case DataTypes.Guid:
                        properties.Add(columnInfo.Name, Guid.NewGuid());
                        break;
                }
            }

            dynamicData.Add(properties.ToDynamic());
        }

        return Task.FromResult(dynamicData);
    }
}