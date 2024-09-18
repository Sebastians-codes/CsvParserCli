using CsvParserCli.Deserializng;

namespace CsvParserCli;

public class CsvParser
{
    private static readonly Deserializer _deserilizer = new();

    public static List<T> Deserializer<T>(string path) where T : new()
    {
        return _deserilizer.Deserialize<T>(path, ',');
    }

    public static List<T> Deserializer<T>(string path, char delimiter) where T : new()
    {
        return _deserilizer.Deserialize<T>(path, delimiter);
    }
}
