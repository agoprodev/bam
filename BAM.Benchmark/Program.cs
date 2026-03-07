// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BAM.ExcerciseSolutions;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BAMBenchmark>();

[ShortRunJob]
[MemoryDiagnoser]
public class BAMBenchmark
{
  readonly string baseUrlFormat = "https://raw.githubusercontent.com/agoprodev/bam-data/refs/heads/main/v1/data_{0}/data_{{0}}_{{1}}.json";

  [Params(10, 20, 50, 100, 200, 500, 1000)]
  public int pages;

  //[Params("Seattle", "Portland", "San Francisco", "Miami")]
  [Params("Chicago")]
  public string city;

  [Params(500, 5000, 10000)]
  public int votes;

  [Benchmark]
  public void Sequential_1() => new Sequential_1(string.Format(baseUrlFormat, pages)).findOutlet(city, votes);

  [Benchmark]
  public void Sequential_2() => new Sequential_2(string.Format(baseUrlFormat, pages)).findOutlet(city, votes);

  [Benchmark]
  public void Parallel_1() => new Parallel_1(string.Format(baseUrlFormat, pages)).findOutlet(city, votes);

  [Benchmark]
  public void Parallel_2() => new Parallel_2(string.Format(baseUrlFormat, pages)).findOutlet(city, votes);

  [Benchmark]
  public void Parallel_3() => new Parallel_3(string.Format(baseUrlFormat, pages)).findOutlet(city, votes);

}