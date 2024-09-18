using CsvParserCli.Deserializing;
using CsvParserCli.Serializing;

namespace CsvParserCli;

public class CsvParser
{
    private static readonly Deserializer _deserilizer = new();
    private static readonly Serializer _serializer = new();

    public static List<T> Deserializer<T>(string path) where T : new()
    {
        return _deserilizer.Deserialize<T>(path, ',');
    }

    public static List<T> Deserializer<T>(string path, char delimiter) where T : new()
    {
        return _deserilizer.Deserialize<T>(path, delimiter);
    }

    public static void Serialize<T>(T oneClass, string path, char delimiter = ',')
    {
        _serializer.Serialize<T>([oneClass], path, delimiter);
    }

    public static void Serialize<T>(IEnumerable<T> objects, string path, char delimiter = ',')
    {
        _serializer.Serialize<T>(objects, path, delimiter);
    }
}
