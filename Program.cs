// validate parameters
using System.Net.Http.Json;
using System.Text.Json;

if (args.Length != 2 || !int.TryParse(args[1], out int votes))
{
  Console.WriteLine("Usage: dotnet run <city> <votes>");
  return;
}
var city = args[0];

Console.WriteLine($"Find outlet for {city} with minimal {votes} votes.");

var httpClient = new HttpClient();
var jsonSerializerOptions = new JsonSerializerOptions
{
  PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
};

string outlet = findOutlet(city, votes);

Console.WriteLine($"Finest outlet for {city} is {outlet}.");

string findOutlet(string city, int voutes)
{
  var urlFormat = $"https://jsonmock.hackerrank.com/api/food_outlets?city={city}&page={{0}}";
  ResultsDto? resultDto = httpClient.GetFromJsonAsync<ResultsDto>(string.Format(urlFormat, 1), jsonSerializerOptions).Result;
  if (resultDto is null || resultDto.Total == 0)
  {
    return ""; // not found
  }
  var finest = findFinestOutletWithMinimalVotes(resultDto.Data, votes);
  var are = new AutoResetEvent(true);
  Parallel.For<OutletDto?>(2, resultDto.TotalPages + 1, 
    //new ParallelOptions {  MaxDegreeOfParallelism = 2}, // this was set for debuging to limit to 2 threads, but in general let runtime to decide the optimal number
    () => null, (page, _, threadFinest) =>  {
    //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Staring Page {page} with threadFinest: {threadFinest}.");
    ResultsDto? batchResult = httpClient.GetFromJsonAsync<ResultsDto>(string.Format(urlFormat, page), jsonSerializerOptions).Result;
    OutletDto? batchFinest = findFinestOutletWithMinimalVotes(resultDto.Data, votes);
    if (threadFinest is null)
    {
      //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Page {page} returning batchFinset: {batchFinest} as no threadFinest");
      return batchFinest;
    }
    if (batchFinest is not null && batchFinest.UserRating.AverageRating > threadFinest.UserRating.AverageRating)
    {
      //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Page {page} returning batchFinset: {batchFinest} as better");
      return batchFinest;
    }
    //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Page {page} returning threadFinest: {threadFinest} as better than batchFinnest");
    return threadFinest;
  }, 
  (threadFinest) =>  {
    if (threadFinest is not null)
    {
      are.WaitOne();
      //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Compare finest {finest} with threadFinest {threadFinest}");
      if (threadFinest is not null && threadFinest.UserRating.AverageRating > threadFinest.UserRating.AverageRating)
      {
        //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ThreadFinest {threadFinest} is better than {finest}");
        finest = threadFinest;
      }
      are.Set();
    }
  });
  Console.WriteLine($"Finest outlet is {finest}.");
  return finest?.Name;
}
OutletDto? findFinestOutletWithMinimalVotes(List<OutletDto> outlets, int votes)
{
  return outlets.AsParallel().Where(o => o.UserRating.Votes >= votes).AsSequential().OrderByDescending(o => o.UserRating.AverageRating).FirstOrDefault();
}

public class ResultsDto
{
  public int Page { get; set;  }
  public int PerPage { get; set;  }
  public int Total { get; set;  }
  public int TotalPages { get; set;  }
  public List<OutletDto> Data { get; set; }
}
public class OutletDto
{
  public string City { get;set; }
  public string Name { get;set; }
  public UserRatingDto UserRating  { get;set; }
  public override string ToString()
  {
    return $"Outlet {Name} rating {UserRating.AverageRating} and votes {UserRating.Votes}";
  }
}
public class UserRatingDto
{
  public decimal AverageRating { get;set; }
  public int Votes { get;set; }
}