using CsvParserCli;

var people = CsvParser.Deserializer<Person>("people.csv");

foreach (Person person in people)
{
    person.Hello();
}