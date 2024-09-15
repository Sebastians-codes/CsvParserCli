using CsvParserCli;

CsvParser csv = new();

List<string[]> people = csv.Read("people.csv", ',', true);

List<Person> listOfPeople = [];

foreach (string[] person in people)
{
    listOfPeople.Add(new Person { Name = person[0], Age = int.Parse(person[1]), City = person[2] });
}

foreach (Person person in listOfPeople)
{
    person.Hello();
}