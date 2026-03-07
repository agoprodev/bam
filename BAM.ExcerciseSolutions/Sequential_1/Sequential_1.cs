using System.Net.Http.Json;
using System.Text.Json;

namespace BAM.ExcerciseSolutions;

public class Sequential_1
{
  readonly HttpClient httpClient = new HttpClient();
  readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
  };
  public string findOutlet(string city, int votes)
  {
    var urlFormat = $"https://jsonmock.hackerrank.com/api/food_outlets?city={city}&page={{0}}";
    ResultsDto? resultDto = httpClient.GetFromJsonAsync<ResultsDto>(string.Format(urlFormat, 1), jsonSerializerOptions).Result;
    if (resultDto is null || resultDto.Total == 0)
    {
      return ""; // not found
    }
    var finest = findFinestOutletWithMinimalVotes(resultDto.Data, votes);
    for (int i = 2; i < resultDto.TotalPages; i++)
    {
      ResultsDto? batchResult = httpClient.GetFromJsonAsync<ResultsDto>(string.Format(urlFormat, 1), jsonSerializerOptions).Result;
      if (batchResult is not null && batchResult.Data is not null && batchResult.Data.Any())
      {
        var batchFinest = findFinestOutletWithMinimalVotes(resultDto.Data, votes);
        if (batchFinest is not null && batchFinest.UserRating.AverageRating > batchFinest.UserRating.AverageRating)
        {
          finest = batchFinest;
        }
      }
    }
    return finest?.Name;
  }
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
  }
  class UserRatingDto
  {
    public decimal AverageRating { get;set; }
    public int Votes { get;set; }
  }
}