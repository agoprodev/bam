// validate parameters
if (args.Length != 2 || !int.TryParse(args[1], out int votes))
{
    Console.WriteLine("Usage: dotnet run <city> <votes>");
    return;
}
var city = args[0];
Console.WriteLine($"Find outlet for {city} with minimal {votes} votes.");

string outlet = findOutlet(city, votes);

Console.WriteLine($"Finest outlet for {city} is {outlet}.");

string findOutlet(string city, int voutes)
{
  return "<TODO>";
}
