using CsvParserCli;

// var people = CsvParser.Deserializer<Person>("people.csv");

// foreach (Person person in people)
// {
//     person.Hello();
// }

CsvParser.Serialize(new Person()
{
    Name = "test",
    Age = 45,
    City = "göte"
}, "test");