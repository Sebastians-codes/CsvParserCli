using CsvParserCli;

var people = CsvParser.Deserialize<Person>("people.csv", ',');

foreach (Person person in people)
{
    person.Hello();
}