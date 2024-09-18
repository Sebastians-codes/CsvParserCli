using CsvParserCli.Deserializing;
using CsvParserCli.Parsing;

namespace CsvParserCli.Deserializng;

internal class Deserializer
{
    private readonly Parser _parser;
    private readonly MemberValueSetter _classSetter;
    private readonly MemberValueGetter _classGetter;

    public Deserializer()
    {
        _parser = new Parser();
        _classSetter = new MemberValueSetter();
        _classGetter = new MemberValueGetter();
    }
    public List<T> Deserialize<T>(string path, char delimiter = ',') where T : new()
    {
        List<T> values = [];

        using var reader = new StreamReader(path);

        if (reader.EndOfStream)
        {
            return values;
        }

        string? headerLine = reader.ReadLine();

        if (string.IsNullOrWhiteSpace(headerLine))
        {
            return values;
        }

        string[] headers = _parser.ParseLine(headerLine, delimiter);
        var memberMap = _classGetter.GetMap<T>();

        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] classProperties = _parser.ParseLine(line, delimiter);
            T newClass = new T();

            for (int i = 0; i < classProperties.Length; i++)
            {
                if (memberMap.TryGetValue(headers[i], out var member))
                {
                    _classSetter.Set(newClass, member, classProperties[i]);
                }
            }

            values.Add(newClass);
        }

        return values;
    }
}
