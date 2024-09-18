using CsvParserCli.Deserializing;
namespace CsvParserCli.Serializing;

internal class Serializer
{
    private readonly MemberValueGetter _classGetter;

    public Serializer()
    {
        _classGetter = new MemberValueGetter();
    }
    public void Serialize<T>(IEnumerable<T> objects, string path, char delimiter = ',')
    {
        var map = _classGetter.GetMap<T>();

        string? headerLine = string.Join($"{delimiter}", map.Keys.ToArray());

        Console.WriteLine(headerLine);

        if (!File.Exists(path))
        {

        }
    }

    private void CreateNewCsvFile<T>(IEnumerable<T> content, string path)
    {
        var contents = new string[content.Count()];

        foreach (var str in content)
        {
        }
    }
}
