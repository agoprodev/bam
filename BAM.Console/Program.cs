using BAM.ExcerciseSolutions;
using System.Text.RegularExpressions;

// validate parameters
if (args.Length != 3 || !Regex.IsMatch(args[0], "^(s[12]|p[1-3])$") || !int.TryParse(args[2], out int votes))
{
  Console.WriteLine("Usage: dotnet run <solution> <city> <votes>");
  Console.WriteLine("Solutions:");
  Console.WriteLine("\ts1 - sequential #1");
  Console.WriteLine("\ts2 - sequential #2 (with thread local state & UTF8JsonReader)");
  Console.WriteLine("\tp1 - parallel #1");
  Console.WriteLine("\tp2 - parallel #2 (with thread local state)");
  Console.WriteLine("\tp3 - parallel #3 (with thread local state & UTF8JsonReader)");
  return;
}
var solution = args[0];
var city = args[1];
Console.WriteLine($"Find outlet for {city} with minimal {votes} votes.");

string outlet = solution switch
{
  "s1" => new Sequential_1().findOutlet(city, votes),
  "s2" => new Sequential_2().findOutlet(city, votes),
  "p1" => new Parallel_1().findOutlet(city, votes),
  "p2" => new Parallel_2().findOutlet(city, votes),
  "p3" => new Parallel_3().findOutlet(city, votes),
  _ => throw new ArgumentException($"Unknown solution: {solution}")
};
if (string.IsNullOrEmpty(outlet))
{
  Console.WriteLine($"No outlet found for {city} with minimal {votes} votes.");
}
else
{
  Console.WriteLine($"Finest outlet for {city} is `{outlet}`.");
}