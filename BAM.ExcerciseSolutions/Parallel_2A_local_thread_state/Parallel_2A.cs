/*
 * based on commit #4e9cb74@main
 */
// validate parameters
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BAM.ExcerciseSolutions;

public class Parallel_2A(string? urlFormat = null)
{
  readonly string urlFormat = urlFormat ?? $"https://jsonmock.hackerrank.com/api/food_outlets?city={{0}}&page={{1}}";
  readonly HttpClient httpClient = new HttpClient();
  readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
  };
  public string findOutlet(string city, int votes)
  {
    ResultsDto? batchResult = httpClient.GetFromJsonAsync<ResultsDto>(string.Format(urlFormat, city, 1), jsonSerializerOptions).Result;
    if (batchResult is null || batchResult.Total == 0)
    {
      return ""; // not found
    }
    int pages = batchResult.TotalPages;
    var allFinest = findFinestOutletWithMinimalVotes(batchResult.Data, votes);
    var are = new AutoResetEvent(true);
    Parallel.For<OutletDto?>(2, pages + 1,
      //new ParallelOptions {  MaxDegreeOfParallelism = 2}, // this was set for debuging to limit to 2 threads, but in general let runtime to decide the optimal number
      () => null, (page, _, threadFinest) => {
        //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Staring Page {page} with threadFinest: {threadFinest}.");
        batchResult = httpClient.GetFromJsonAsync<ResultsDto>(string.Format(urlFormat, city, page), jsonSerializerOptions).Result;
        OutletDto? batchFinest = findFinestOutletWithMinimalVotes(batchResult.Data, votes);
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
    (threadFinest) => {
      if (threadFinest is not null)
      {
        are.WaitOne();
        //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Compare finest {finest} with threadFinest {threadFinest}");
        if (threadFinest is not null && threadFinest.UserRating.AverageRating > (allFinest?.UserRating.AverageRating ?? 0))
        {
          //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ThreadFinest {threadFinest} is better than {finest}");
          allFinest = threadFinest;
        }
        are.Set();
      }
    });
    Console.WriteLine($"Finest outlet is {allFinest}.");
    return allFinest?.Name;
  }
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  OutletDto? findFinestOutletWithMinimalVotes(List<OutletDto> outlets, int votes)
  {
    return outlets.Where(o => o.UserRating.Votes >= votes).OrderByDescending(o => o.UserRating.AverageRating).FirstOrDefault();
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
