namespace CsvParserCli;

public class Person
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? City { get; set; }

    public void Hello() =>
        Console.WriteLine($"My name is {Name}, and i am {Age}years old. And i live in {City}.");
}
