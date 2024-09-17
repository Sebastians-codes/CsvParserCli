using System.Reflection;
using System.Text;

namespace CsvParserCli;

public class CsvParser()
{
    public static List<T> Deserialize<T>(string path, char delimiter = ',') where T : new()
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

        string[] headers = ParseLine(headerLine, delimiter);
        var memberMap = GetMemberMap<T>();

        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] classProperties = ParseLine(line, delimiter);
            T newClass = new T();

            for (int i = 0; i < classProperties.Length; i++)
            {
                if (memberMap.TryGetValue(headers[i], out var member))
                {
                    SetMemberValues(newClass, member, classProperties[i]);
                }
            }

            values.Add(newClass);
        }

        return values;
    }

    private static string[] ParseLine(string line, char delimiter)
    {
        int lineLength = line.Length;
        List<string> fields = [];
        StringBuilder currentField = new();
        bool firstEscapeQuote = false;
        bool secondEscapeQuote = false;

        for (int i = 0; i < lineLength; i++)
        {
            char currentChar = line[i];

            if (currentChar == '"')
            {
                if (i > 0 && line[i - 1] == currentChar)
                {
                    if (firstEscapeQuote)
                    {
                        firstEscapeQuote = false;
                    }

                    if (!firstEscapeQuote && !secondEscapeQuote)
                    {
                        currentField.Append(currentChar);
                        firstEscapeQuote = true;
                        secondEscapeQuote = true;
                    }
                    else if (secondEscapeQuote)
                    {
                        currentField.Append(currentChar);
                        secondEscapeQuote = false;
                    }
                }
                else
                {
                    firstEscapeQuote = !firstEscapeQuote;
                }
            }
            else if (currentChar != delimiter || firstEscapeQuote)
            {
                currentField.Append(currentChar);
            }
            else
            {
                fields.Add(string.Join("", currentField));
                currentField.Clear();
            }
        }

        // Sista stringen måste läggas till i efterhand eftersom i loopen så läggs stringsen endast till i listan 
        // när firstEscape är false och currentChar är delimitern.
        if (currentField.Length > 0)
        {
            fields.Add(string.Join("", currentField));
        }

        return fields.ToArray();
    }

    private static void SetMemberValues<T>(T obj, MemberInfo member, string value)
    {
        try
        {
            object convertedValue;
            Type memberType;

            if (member is PropertyInfo property)
            {
                memberType = property.PropertyType;
            }
            else if (member is FieldInfo field)
            {
                memberType = field.FieldType;
            }
            else
            {
                throw new InvalidOperationException("Member is neither a property nor a field.");
            }

            if (memberType == typeof(string))
            {
                convertedValue = value;
            }
            else if (memberType.IsEnum)
            {
                convertedValue = Enum.Parse(memberType, value, true);
            }
            else
            {
                convertedValue = Convert.ChangeType(value, memberType);
            }

            if (member is PropertyInfo prop)
            {
                prop.SetValue(obj, convertedValue);
            }
            else if (member is FieldInfo field)
            {
                field.SetValue(obj, convertedValue);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting member {member.Name}: {ex.Message}");
        }
    }

    private static Dictionary<string, MemberInfo> GetMemberMap<T>()
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
