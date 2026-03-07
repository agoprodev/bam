/*
 * based on commit #6a5b973@main
 */
using System.Net.Http.Json;
using System.Text.Json;

namespace BAM.ExcerciseSolutions;

public class Parallel_1(string? urlFormat = null)
{
  readonly string urlFormat = urlFormat ?? $"https://jsonmock.hackerrank.com/api/food_outlets?city={{0}}&page={{1}}";
  readonly HttpClient httpClient = new HttpClient();
  readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
  };
  
  public string findOutlet(string city, int votes)
  {
    
    ResultsDto? resultDto = httpClient.GetFromJsonAsync<ResultsDto>(string.Format(urlFormat, city, 1), jsonSerializerOptions).Result;
    if (resultDto is null || resultDto.Total == 0)
    {
      return ""; // not found
    }
    var finest = findFinestOutletWithMinimalVotes(resultDto.Data, votes);
    var are = new AutoResetEvent(true);
    Parallel.For(2, resultDto.TotalPages + 1, (int page) =>
    {
      ResultsDto? batchResult = httpClient.GetFromJsonAsync<ResultsDto>(string.Format(urlFormat, city, page), jsonSerializerOptions).Result;
      if (batchResult is not null && batchResult.Data is not null && batchResult.Data.Any())
      {
        var batchFinest = findFinestOutletWithMinimalVotes(resultDto.Data, votes);
        are.WaitOne();
        if (batchFinest is not null && batchFinest.UserRating.AverageRating > (finest?.UserRating.AverageRating ?? 0))
        {
          finest = batchFinest;
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
  class ResultsDto
  {
    public int Page { get; set;  }
    public int PerPage { get; set;  }
    public int Total { get; set;  }
    public int TotalPages { get; set;  }
    public List<OutletDto> Data { get; set; }
  }
  class OutletDto
  {
    public string City { get;set; }
    public string Name { get;set; }
    public UserRatingDto UserRating  { get;set; }
    public override string ToString()
    {
      return $"Outlet {Name} rating {UserRating.AverageRating} and votes {UserRating.Votes}";
    }
  }
  class UserRatingDto
  {
    public decimal AverageRating { get;set; }
    public int Votes { get;set; }
  }
}
