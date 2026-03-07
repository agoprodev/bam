using BAM.ExcerciseSolutions;
using System.Diagnostics;
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

var baseUrl = "https://raw.githubusercontent.com/agoprodev/bam-data/refs/heads/main/v1/data_100/data_{0}_{1}.json";
//Debugger.Launch();
Stopwatch stopwatch = Stopwatch.StartNew();
string outlet = solution switch
{
  "s1" => new Sequential_1(baseUrl).findOutlet(city, votes),
  "s2" => new Sequential_2(baseUrl).findOutlet(city, votes),
  "p1" => new Parallel_1(baseUrl).findOutlet(city, votes),
  "p2" => new Parallel_2(baseUrl).findOutlet(city, votes),
  "p3" => new Parallel_3(baseUrl).findOutlet(city, votes),
  _ => throw new ArgumentException($"Unknown solution: {solution}")
};
stopwatch.Stop();
Console.WriteLine($"Execution time: {stopwatch.Elapsed}.");
Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} ms.");

if (string.IsNullOrEmpty(outlet))
{
  Console.WriteLine($"No outlet found for {city} with minimal {votes} votes.");
}
else
{
  Console.WriteLine($"Finest outlet for {city} is `{outlet}`.");
}