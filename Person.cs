namespace CsvParserCli;

public class Person
{
    public string? Name { get; init; }
    public int Age { get; init; }
    public string? City { get; set; }

    public void Hello() =>
        Console.WriteLine($"My name is {Name}, and i am {Age}years old. And i live in {City}.");
}
