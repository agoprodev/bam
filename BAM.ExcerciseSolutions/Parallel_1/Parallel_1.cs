/*
 * based on commit #6a5b973@main
 */

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
  Parallel.For(2, resultDto.TotalPages + 1, (int page) =>
  {
    ResultsDto? batchResult = httpClient.GetFromJsonAsync<ResultsDto>(string.Format(urlFormat, page), jsonSerializerOptions).Result;
    if (batchResult is not null && batchResult.Data is not null && batchResult.Data.Any())
    {
      var batchFinest = findFinestOutletWithMinimalVotes(resultDto.Data, votes);
      are.WaitOne();
      if (batchFinest is not null && batchFinest.UserRating.AverageRating > batchFinest.UserRating.AverageRating)
      {
        finest = batchFinest;
      }
      are.Set();
    }

  });
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
}
public class UserRatingDto
{
  public decimal AverageRating { get;set; }
  public int Votes { get;set; }
}