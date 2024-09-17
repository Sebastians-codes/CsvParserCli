using System.Text;

namespace CsvParserCli;

public class CsvParser()
{
    public List<string[]> Read(string path)
    {
        char delimiter = ',';
        if (!File.Exists(path))
        {
            return [];
        }

        List<string[]> parsedCsv = [];
        string[] csvContent = File.ReadAllLines(path);

        if (csvContent.Length == 0)
        {
            return [];
        }

        for (int i = 1; i < csvContent.Length; i++)
        {
            parsedCsv.Add(ParseLine(csvContent[i], delimiter));
        }

        return parsedCsv;
    }

    public List<string[]> Read(string path, char delimiter)
    {
        if (!File.Exists(path))
        {
            return [];
        }

        List<string[]> parsedCsv = [];
        string[] csvContent = File.ReadAllLines(path);

        if (csvContent.Length == 0)
        {
            return [];
        }

        for (int i = 1; i < csvContent.Length; i++)
        {
            parsedCsv.Add(ParseLine(csvContent[i], delimiter));
        }

        return parsedCsv;
    }

    public List<string[]> Read(string path, bool header)
    {
        int startIndex = header ? 1 : 0;
        char delimiter = ',';
        if (!File.Exists(path))
        {
            return [];
        }

        List<string[]> parsedCsv = [];
        string[] csvContent = File.ReadAllLines(path);

        if (csvContent.Length == 0)
        {
            return [];
        }

        for (int i = startIndex; i < csvContent.Length; i++)
        {
            parsedCsv.Add(ParseLine(csvContent[i], delimiter));
        }

        return parsedCsv;
    }

    public List<string[]> Read(string path, char delimiter, bool header)
    {
        int startIndex = header ? 1 : 0;
        if (!File.Exists(path))
        {
            return [];
        }

        List<string[]> parsedCsv = [];
        string[] csvContent = File.ReadAllLines(path);

        if (csvContent.Length == 0)
        {
            return [];
        }

        for (int i = startIndex; i < csvContent.Length; i++)
        {
            parsedCsv.Add(ParseLine(csvContent[i], delimiter));
        }

        return parsedCsv;
    }

    public string[] ParseLine(string line, char delimiter)
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
}
