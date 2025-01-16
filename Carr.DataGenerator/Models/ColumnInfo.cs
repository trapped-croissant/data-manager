using Carr.DataGenerator.Data;

namespace Carr.DataGenerator.Models;

public record ColumnInfo(string Name,DataTypes DataType,  bool IsNullable, int? Size);