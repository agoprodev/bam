// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BAM.ExcerciseSolutions;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BAMBenchmark>();

[ShortRunJob]
[RankColumn]
[MemoryDiagnoser]
public class BAMBenchmark
{
  [Params("Seattle", "Portland", "San Francisco", "Miami")]
  public string city;

  [Params(100, 200, 500, 1000)]
  public int votes;

  [Benchmark]
  public void Sequential_1() => new Sequential_1().findOutlet(city, votes);

  [Benchmark]
  public void Sequential_1A() => new Sequential_1A().findOutlet(city, votes);

  [Benchmark]
  public void Sequential_2() => new Sequential_2().findOutlet(city, votes);

  [Benchmark]
  public void Parallel_1() => new Parallel_1().findOutlet(city, votes);

  [Benchmark]
  public void Parallel_1A() => new Parallel_1A().findOutlet(city, votes);

  [Benchmark]
  public void Parallel_2() => new Parallel_2().findOutlet(city, votes);

  [Benchmark]
  public void Parallel_2A() => new Parallel_2A().findOutlet(city, votes);

  [Benchmark]
  public void Parallel_3() => new Parallel_3().findOutlet(city, votes);

}