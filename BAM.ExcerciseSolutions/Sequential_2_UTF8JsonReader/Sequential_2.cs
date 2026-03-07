using System.Net.Http.Json;
using System.Text.Json;

namespace BAM.ExcerciseSolutions;

public class Sequential_2
{
  readonly HttpClient httpClient = new HttpClient();
  readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
  };
  public string findOutlet(string city, int votes)
  {
    var urlFormat = $"https://jsonmock.hackerrank.com/api/food_outlets?city={city}&page={{0}}";
    byte[] jsonBatchOne = httpClient.GetByteArrayAsync(string.Format(urlFormat, 1)).Result;
    if (jsonBatchOne is null || jsonBatchOne.Length == 0)
    {
      return ""; // not found
    }
    OutletInfo? finest = findFinestOutletWithMinimalVotesFirst(jsonBatchOne, votes, out int pages);
    for (int i = 2; i < pages; i++)
    {
      byte[] jsonBatch = httpClient.GetByteArrayAsync(string.Format(urlFormat, i)).Result;
      OutletInfo? batchFinest = findFinestOutletWithMinimalVotesFirst(jsonBatch, votes, out int _);
      if (batchFinest is not null && batchFinest.AverageRating > (finest?.AverageRating ?? 0))
      {

        finest = batchFinest;
      }
    }
    Console.WriteLine($"Finest outlet is {finest}.");
    return finest?.Name;
  }
  OutletInfo? findFinestOutletWithMinimalVotesFirst(byte[] jsonData, int votes, out int pages)
  {
    pages = 0;
    OutletInfo? foundFinestInfo = null;
    var reader = new Utf8JsonReader(jsonData);

    bool dataElementFound = false;

    // iterate over root element properties
    while (reader.Read())
    {
      // here assumption is "data" is after "total_pages"
      if (reader.TokenType == JsonTokenType.PropertyName)
      {
        var propertyName = reader.GetString();
        if (propertyName == "total_pages")
        {
          reader.Read();
          pages = reader.GetInt32();
        }
        if (propertyName == "data")
        {
          dataElementFound = true; // fould "data" element
          break;
        }
      }
    }
    if (dataElementFound)
    {
      // iterate over "data" collection
      bool finished = false;
      var outletInfo = new OutletInfo();
      int processingElement = 0; // 1 means array, 2 means processing outlet object, 3 means processing outlet's rating
      while (reader.Read())
      {
        if (reader.TokenType == JsonTokenType.PropertyName)
        {
          var propertyName = reader.GetString();
          if (processingElement == 2)
          {
            if (propertyName == "name")
            {
              reader.Read();
              outletInfo.Name = reader.GetString();
            }
          }
          else if (processingElement == 3)
          {
            if (propertyName == "average_rating")
            {
              reader.Read();
              outletInfo.AverageRating = reader.GetDecimal();
            }
            else if (propertyName == "votes")
            {
              reader.Read();
              outletInfo.Votes = reader.GetInt32();
            }
          }
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
          ++processingElement;
          if (processingElement == 2)
          {
            // reset properties
            outletInfo.Name = null;
            outletInfo.Votes = 0;
            outletInfo.AverageRating = 0;
          }
        }
        else if (reader.TokenType == JsonTokenType.EndObject)
        {
          if (processingElement == 2)
          {
            // compare with finest
            if (outletInfo.IsValid(mininmalVotes: votes))
            {
              if (foundFinestInfo is null || outletInfo.AverageRating > foundFinestInfo.AverageRating)
              {
                foundFinestInfo = outletInfo with
                {
                }; // create a copy
              }
            }
          }
          --processingElement;
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
          ++processingElement; // processing array
          continue;
        }
        else if (reader.TokenType == JsonTokenType.EndArray)
        {
          --processingElement; // finsihed array processing
          break; // we finish processing array here 
        }
      }
    }
    return foundFinestInfo;
  }
  record OutletInfo
  {
    public string Name { get;set; }
    public decimal AverageRating { get;set; }
    public int Votes { get;set; }  

    public bool IsValid(int mininmalVotes) => !string.IsNullOrEmpty(Name) && Votes >= mininmalVotes && AverageRating > 0;
  }
}
