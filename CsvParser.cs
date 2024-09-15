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
                /*
                    Om currentChar är " 
                        Om i är mer än 0 och föregående char är lika med currentChar
                            Om första escaped trigger
                                firstEscape = false
                            Om firstEscape och secondEscape == false
                                Lägg till current char i buildern
                                firstEscape och secondEscape = true
                            Annars Om i är mindre än längden av raden och secondEscape == true
                                Lägg till current char i buildern
                                secondEscape = false

                        // Detta är för att set till att " can escapa " så vi kan parsa text
                        // som ser ut såhär ""Detta är ett , Exempel"" resulterar i "Detta är ett , Exempel"
                        //  ""Detta är ett , Exempel""  första " sätter first till true andra " sätter first till false först
                        // och sen är både first och second är false så då blir båda true
                        // nästa sätter first till false och den efter det triggrar bara på second
                        // and the loop continues

                        Annars
                            firstEscape = not firstEscape
                        
                        // Detta är för att firstEscape sätter att Parsern ska ignorera delimitern.
                */
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
                    else if (i < lineLength && secondEscapeQuote)
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
