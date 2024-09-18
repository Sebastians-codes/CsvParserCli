using System.Reflection;

namespace CsvParserCli.Deserializing;

internal class MemberValueGetter
{
    protected internal Dictionary<string, MemberInfo> GetMap<T>()
    {
        var type = typeof(T);
        var map = new Dictionary<string, MemberInfo>(StringComparer.OrdinalIgnoreCase);

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.CanWrite)
            {
                map[prop.Name] = prop;
            }
        }

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            map[field.Name] = field;
        }

        return map;
    }
}
