using System.Dynamic;

namespace Carr.DataGenerator.Extensions;

public static class DictionaryExtensions
{
    public static dynamic ToDynamic(this IDictionary<string, object> dictionary)
    {
        var expandoObject = new ExpandoObject();
        ICollection<KeyValuePair<string, object>> expandoObjectCollection = expandoObject!;

        foreach (var item in dictionary)
        {
            expandoObjectCollection.Add(item);
        }

        dynamic dynamicObject = expandoObject;

        return dynamicObject;
    }
}