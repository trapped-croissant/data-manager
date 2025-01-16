using System.Dynamic;

namespace Carr.DataGenerator.Extensions;

public static class DictionaryExtensions
{
    public static dynamic ToDynamic(this IDictionary<string, object> dictionary)
    {
        var dynamicList = new List<dynamic>();
        
        var eo = new ExpandoObject();
        var eoColl = (ICollection<KeyValuePair<string, object>>)eo!;

        foreach (var item in dictionary)
        {
            eoColl.Add(item);
        }

        dynamic eoDynamic = eo;

        return eoDynamic;
    }
}