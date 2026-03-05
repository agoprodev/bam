// validate parameters
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
  byte[] jsonBatchOne = httpClient.GetByteArrayAsync(string.Format(urlFormat, 1)).Result;
  if (jsonBatchOne is null || jsonBatchOne.Length == 0)
  {
    return ""; // not found
  }
  OutletInfo? allFinest = findFinestOutletWithMinimalVotesFirst(jsonBatchOne, votes, out int pages);
  var are = new AutoResetEvent(true);
  Parallel.For<OutletInfo?>(2, pages + 1,
    //new ParallelOptions {  MaxDegreeOfParallelism = 2}, // this was set for debuging to limit to 2 threads, but in general let runtime to decide the optimal number
    () => null, (page, _, threadFinest) =>
    {
      //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Staring Page {page} with threadFinest: {threadFinest}.");
      byte[] jsonBatch = httpClient.GetByteArrayAsync(string.Format(urlFormat, 1)).Result;
      OutletInfo? batchFinest = findFinestOutletWithMinimalVotesFirst(jsonBatch, votes, out int _);
      if (threadFinest is null)
      {
        //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Page {page} returning batchFinset: {batchFinest} as no threadFinest");
        return batchFinest;
      }
      if (batchFinest is not null && batchFinest.AverageRating > threadFinest.AverageRating)
      {
        //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Page {page} returning batchFinset: {batchFinest} as better");
        return batchFinest;
      }
      //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Page {page} returning threadFinest: {threadFinest} as better than batchFinnest");
      return threadFinest;
    },
  (threadFinest) =>
  {
    if (threadFinest is not null)
    {
      are.WaitOne();
      //Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Compare finest {finest} with threadFinest {threadFinest}");
      if (threadFinest is not null && threadFinest.AverageRating > allFinest.AverageRating)
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
public record OutletInfo
{
  public string Name { get;set; }
  public decimal AverageRating { get;set; }
  public int Votes { get;set; }  

  public bool IsValid(int mininmalVotes) => !string.IsNullOrEmpty(Name) && Votes >= mininmalVotes && AverageRating > 0;
}
