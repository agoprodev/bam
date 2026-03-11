# Usage

Usage: `dotnet run <city> <votes>`


## Baranches (you are now in `main` branch)
1. [`main`](https://github.com/agoprodev/bam/tree/main) - with DTO
1. [`utf8jsonreader`](https://github.com/agoprodev/bam/tree/utf8jsonreader) - using `Utf8JsonReader` instead of DTO, faster but much relies on structure
1. [all-in-one](https://github.com/agoprodev/bam/tree/all-in-one) - ALL in One (with hacker rank endpoint)
2. [all-in-one-my-endpoint](https://github.com/agoprodev/bam/tree/all-in-one-my-endpoint) - using my generated data and put in GH: https://github.com/agoprodev/bam-data/tree/main/v1


## Benchamrk Results

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
AMD Ryzen 7 PRO 5850U with Radeon Graphics 1.90GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.304
  [Host]   : .NET 9.0.8 (9.0.8, 9.0.825.36511), X64 RyuJIT x86-64-v3
  ShortRun : .NET 9.0.8 (9.0.8, 9.0.825.36511), X64 RyuJIT x86-64-v3

Job=ShortRun  IterationCount=3  LaunchCount=1 WarmupCount=3

| Method        | city          | votes | Mean     | Error       | StdDev    | Rank | Allocated |
|-------------- |-------------- |------ |---------:|------------:|----------:|-----:|----------:|
| Sequential_1  | Miami         | 100   | 749.2 ms |   184.60 ms |  10.12 ms |    2 | 162.27 KB |
| Sequential_1A | Miami         | 100   | 724.8 ms |   587.07 ms |  32.18 ms |    2 | 113.86 KB |
| Sequential_2  | Miami         | 100   | 756.4 ms |    40.18 ms |   2.20 ms |    2 |  79.14 KB |
| Parallel_1    | Miami         | 100   | 724.9 ms |   140.22 ms |   7.69 ms |    2 | 527.86 KB |
| Parallel_1A   | Miami         | 100   | 750.8 ms |   219.49 ms |  12.03 ms |    2 | 196.72 KB |
| Parallel_2    | Miami         | 100   | 747.3 ms |   162.66 ms |   8.92 ms |    2 | 479.93 KB |
| Parallel_2A   | Miami         | 100   | 825.7 ms | 2,697.67 ms | 147.87 ms |    2 | 149.35 KB |
| Parallel_3    | Miami         | 100   | 740.0 ms |   342.84 ms |  18.79 ms |    2 |  90.01 KB |
|||||||||
| Sequential_1  | Miami         | 200   | 714.0 ms |   299.01 ms |  16.39 ms |    2 | 113.57 KB |
| Sequential_1A | Miami         | 200   | 831.9 ms | 2,650.42 ms | 145.28 ms |    2 | 194.37 KB |
| Sequential_2  | Miami         | 200   | 692.8 ms |    72.11 ms |   3.95 ms |    2 |  55.38 KB |
| Parallel_1    | Miami         | 200   | 714.4 ms |   305.52 ms |  16.75 ms |    2 | 528.31 KB |
| Parallel_1A   | Miami         | 200   | 746.8 ms |   233.90 ms |  12.82 ms |    2 | 197.89 KB |
| Parallel_2    | Miami         | 200   | 737.2 ms |   267.28 ms |  14.65 ms |    2 | 480.13 KB |
| Parallel_2A   | Miami         | 200   | 748.2 ms |   281.77 ms |  15.44 ms |    2 | 150.52 KB |
| Parallel_3    | Miami         | 200   | 727.6 ms |    49.06 ms |   2.69 ms |    2 |  90.77 KB |
|||||||||
| Sequential_1  | Miami         | 500   | 736.2 ms |   127.20 ms |   6.97 ms |    2 | 113.95 KB |
| Sequential_1A | Miami         | 500   | 745.8 ms |   323.74 ms |  17.75 ms |    2 | 114.16 KB |
| Sequential_2  | Miami         | 500   | 838.0 ms | 2,442.04 ms | 133.86 ms |    2 |  55.42 KB |
| Parallel_1    | Miami         | 500   | 724.4 ms |   425.23 ms |  23.31 ms |    2 | 480.59 KB |
| Parallel_1A   | Miami         | 500   | 732.1 ms |   290.79 ms |  15.94 ms |    2 | 149.07 KB |
| Parallel_2    | Miami         | 500   | 738.9 ms |   203.41 ms |  11.15 ms |    2 | 479.63 KB |
| Parallel_2A   | Miami         | 500   | 743.3 ms |   295.86 ms |  16.22 ms |    2 |  149.2 KB |
| Parallel_3    | Miami         | 500   | 742.5 ms |   322.42 ms |  17.67 ms |    2 |  90.38 KB |
|||||||||
| Sequential_1  | Miami         | 1000  | 751.0 ms |   472.92 ms |  25.92 ms |    2 | 114.23 KB |
| Sequential_1A | Miami         | 1000  | 756.5 ms |   344.22 ms |  18.87 ms |    2 |  162.2 KB |
| Sequential_2  | Miami         | 1000  | 752.7 ms |   336.65 ms |  18.45 ms |    2 |  55.42 KB |
| Parallel_1    | Miami         | 1000  | 745.0 ms |   192.10 ms |  10.53 ms |    2 | 479.27 KB |
| Parallel_1A   | Miami         | 1000  | 761.0 ms |   133.03 ms |   7.29 ms |    2 | 148.95 KB |
| Parallel_2    | Miami         | 1000  | 747.8 ms |   162.41 ms |   8.90 ms |    2 | 479.58 KB |
| Parallel_2A   | Miami         | 1000  | 755.8 ms |    39.09 ms |   2.14 ms |    2 | 148.99 KB |
| Parallel_3    | Miami         | 1000  | 744.1 ms |   206.09 ms |  11.30 ms |    2 |  90.27 KB |
|||||||||
| Sequential_1  | Portland      | 100   | 751.1 ms |   279.72 ms |  15.33 ms |    2 | 114.14 KB |
| Sequential_1A | Portland      | 100   | 736.6 ms |    19.74 ms |   1.08 ms |    2 | 114.22 KB |
| Sequential_2  | Portland      | 100   | 742.6 ms |   307.87 ms |  16.88 ms |    2 |   54.8 KB |
| Parallel_1    | Portland      | 100   | 747.1 ms |   198.71 ms |  10.89 ms |    2 | 479.65 KB |
| Parallel_1A   | Portland      | 100   | 780.8 ms |   427.70 ms |  23.44 ms |    2 | 197.06 KB |
| Parallel_2    | Portland      | 100   | 770.8 ms |   717.54 ms |  39.33 ms |    2 | 536.13 KB |
| Parallel_2A   | Portland      | 100   | 745.7 ms |   266.53 ms |  14.61 ms |    2 | 149.24 KB |
| Parallel_3    | Portland      | 100   | 736.9 ms |   182.18 ms |   9.99 ms |    2 |  89.92 KB |
|||||||||
| Sequential_1  | Portland      | 200   | 833.1 ms | 2,551.99 ms | 139.88 ms |    2 | 115.07 KB |
| Sequential_1A | Portland      | 200   | 751.9 ms |   336.16 ms |  18.43 ms |    2 | 162.48 KB |
| Sequential_2  | Portland      | 200   | 758.2 ms |   202.10 ms |  11.08 ms |    2 |  55.01 KB |
| Parallel_1    | Portland      | 200   | 757.0 ms |   118.00 ms |   6.47 ms |    2 |  480.2 KB |
| Parallel_1A   | Portland      | 200   | 742.5 ms |   230.00 ms |  12.61 ms |    2 | 149.42 KB |
| Parallel_2    | Portland      | 200   | 737.3 ms |   174.62 ms |   9.57 ms |    2 | 479.91 KB |
| Parallel_2A   | Portland      | 200   | 735.8 ms |   170.28 ms |   9.33 ms |    2 | 149.55 KB |
| Parallel_3    | Portland      | 200   | 724.5 ms |   648.93 ms |  35.57 ms |    2 | 122.91 KB |
|||||||||
| Sequential_1  | Portland      | 500   | 743.9 ms |   284.83 ms |  15.61 ms |    2 | 113.72 KB |
| Sequential_1A | Portland      | 500   | 745.5 ms |   347.46 ms |  19.05 ms |    2 | 114.51 KB |
| Sequential_2  | Portland      | 500   | 753.5 ms |   298.64 ms |  16.37 ms |    2 |   54.9 KB |
| Parallel_1    | Portland      | 500   | 754.7 ms |   135.55 ms |   7.43 ms |    2 | 479.08 KB |
| Parallel_1A   | Portland      | 500   | 745.4 ms |   473.24 ms |  25.94 ms |    2 | 197.42 KB |
| Parallel_2    | Portland      | 500   | 730.5 ms |   418.72 ms |  22.95 ms |    2 | 479.35 KB |
| Parallel_2A   | Portland      | 500   | 729.8 ms |    25.28 ms |   1.39 ms |    2 | 149.42 KB |
| Parallel_3    | Portland      | 500   | 724.3 ms |   166.78 ms |   9.14 ms |    2 |  90.02 KB |
|||||||||
| Sequential_1  | Portland      | 1000  | 755.1 ms |   356.69 ms |  19.55 ms |    2 | 114.43 KB |
| Sequential_1A | Portland      | 1000  | 737.0 ms |   563.14 ms |  30.87 ms |    2 | 114.14 KB |
| Sequential_2  | Portland      | 1000  | 735.8 ms |    38.18 ms |   2.09 ms |    2 |   54.4 KB |
| Parallel_1    | Portland      | 1000  | 725.9 ms |   292.90 ms |  16.06 ms |    2 | 526.73 KB |
| Parallel_1A   | Portland      | 1000  | 745.4 ms |   172.78 ms |   9.47 ms |    2 | 158.34 KB |
| Parallel_2    | Portland      | 1000  | 725.0 ms |   238.30 ms |  13.06 ms |    2 | 479.17 KB |
| Parallel_2A   | Portland      | 1000  | 704.2 ms |   209.76 ms |  11.50 ms |    2 | 149.92 KB |
| Parallel_3    | Portland      | 1000  | 731.3 ms |   564.34 ms |  30.93 ms |    2 |  89.64 KB |
|||||||||
| Sequential_1  | San Francisco | 100   | 400.3 ms | 1,542.44 ms |  84.55 ms |    1 |  90.98 KB |
| Sequential_1A | San Francisco | 100   | 378.1 ms |   144.05 ms |   7.90 ms |    1 |  90.98 KB |
| Sequential_2  | San Francisco | 100   | 366.7 ms |    30.53 ms |   1.67 ms |    1 |  26.01 KB |
| Parallel_1    | San Francisco | 100   | 377.2 ms |   166.33 ms |   9.12 ms |    1 |  91.04 KB |
| Parallel_1A   | San Francisco | 100   | 368.8 ms |    26.15 ms |   1.43 ms |    1 |  91.04 KB |
| Parallel_2    | San Francisco | 100   | 368.3 ms |    12.95 ms |   0.71 ms |    1 |  91.04 KB |
| Parallel_2A   | San Francisco | 100   | 370.3 ms |    80.69 ms |   4.42 ms |    1 |  139.5 KB |
| Parallel_3    | San Francisco | 100   | 361.1 ms |   166.47 ms |   9.12 ms |    1 |  26.52 KB |
|||||||||
| Sequential_1  | San Francisco | 200   | 368.4 ms |    29.13 ms |   1.60 ms |    1 |  90.98 KB |
| Sequential_1A | San Francisco | 200   | 364.1 ms |   394.17 ms |  21.61 ms |    1 |  90.98 KB |
| Sequential_2  | San Francisco | 200   | 379.0 ms |   181.71 ms |   9.96 ms |    1 |  58.45 KB |
| Parallel_1    | San Francisco | 200   | 383.0 ms |    88.11 ms |   4.83 ms |    1 |  91.04 KB |
| Parallel_1A   | San Francisco | 200   | 383.3 ms |   203.00 ms |  11.13 ms |    1 |  91.04 KB |
| Parallel_2    | San Francisco | 200   | 373.4 ms |   328.31 ms |  18.00 ms |    1 |  91.04 KB |
| Parallel_2A   | San Francisco | 200   | 362.3 ms |   188.36 ms |  10.32 ms |    1 |  91.04 KB |
| Parallel_3    | San Francisco | 200   | 355.9 ms |   175.04 ms |   9.59 ms |    1 |  26.52 KB |
|||||||||
| Sequential_1  | San Francisco | 500   | 371.2 ms |    28.00 ms |   1.53 ms |    1 |  90.98 KB |
| Sequential_1A | San Francisco | 500   | 371.2 ms |    59.40 ms |   3.26 ms |    1 |  90.98 KB |
| Sequential_2  | San Francisco | 500   | 379.1 ms |   200.19 ms |  10.97 ms |    1 |  25.98 KB |
| Parallel_1    | San Francisco | 500   | 362.8 ms |   193.15 ms |  10.59 ms |    1 |  91.04 KB |
| Parallel_1A   | San Francisco | 500   | 382.5 ms |   190.95 ms |  10.47 ms |    1 |  91.04 KB |
| Parallel_2    | San Francisco | 500   | 373.5 ms |   155.68 ms |   8.53 ms |    1 |  91.04 KB |
| Parallel_2A   | San Francisco | 500   | 374.4 ms |   312.00 ms |  17.10 ms |    1 |  91.04 KB |
| Parallel_3    | San Francisco | 500   | 361.9 ms |   159.87 ms |   8.76 ms |    1 |  26.52 KB |
|||||||||
| Sequential_1  | San Francisco | 1000  | 376.6 ms |   153.94 ms |   8.44 ms |    1 |  90.98 KB |
| Sequential_1A | San Francisco | 1000  | 364.4 ms |   151.31 ms |   8.29 ms |    1 |  90.98 KB |
| Sequential_2  | San Francisco | 1000  | 383.4 ms |    47.25 ms |   2.59 ms |    1 |  58.45 KB |
| Parallel_1    | San Francisco | 1000  | 379.3 ms |   161.22 ms |   8.84 ms |    1 |  91.04 KB |
| Parallel_1A   | San Francisco | 1000  | 374.6 ms |   201.80 ms |  11.06 ms |    1 |  91.04 KB |
| Parallel_2    | San Francisco | 1000  | 412.1 ms | 1,099.03 ms |  60.24 ms |    1 |  91.04 KB |
| Parallel_2A   | San Francisco | 1000  | 376.9 ms |   249.69 ms |  13.69 ms |    1 |  91.04 KB |
| Parallel_3    | San Francisco | 1000  | 370.9 ms |   147.33 ms |   8.08 ms |    1 |  26.52 KB |
|||||||||
| Sequential_1  | Seattle       | 100   | 831.1 ms | 2,624.43 ms | 143.85 ms |    2 |  114.3 KB |
| Sequential_1A | Seattle       | 100   | 731.6 ms |   597.21 ms |  32.74 ms |    2 | 117.27 KB |
| Sequential_2  | Seattle       | 100   | 766.1 ms |   505.83 ms |  27.73 ms |    2 |  55.47 KB |
| Parallel_1    | Seattle       | 100   | 734.4 ms |   296.21 ms |  16.24 ms |    2 | 479.91 KB |
| Parallel_1A   | Seattle       | 100   | 729.4 ms |   504.28 ms |  27.64 ms |    2 | 149.11 KB |
| Parallel_2    | Seattle       | 100   | 715.6 ms |   604.23 ms |  33.12 ms |    2 | 480.31 KB |
| Parallel_2A   | Seattle       | 100   | 713.2 ms |   220.72 ms |  12.10 ms |    2 | 149.63 KB |
| Parallel_3    | Seattle       | 100   | 732.8 ms |   227.63 ms |  12.48 ms |    2 |  89.96 KB |
|||||||||
| Sequential_1  | Seattle       | 200   | 745.6 ms |   377.67 ms |  20.70 ms |    2 | 162.19 KB |
| Sequential_1A | Seattle       | 200   | 797.0 ms | 1,475.75 ms |  80.89 ms |    2 | 114.66 KB |
| Sequential_2  | Seattle       | 200   | 757.7 ms |   269.46 ms |  14.77 ms |    2 |  54.89 KB |
| Parallel_1    | Seattle       | 200   | 770.0 ms | 1,196.98 ms |  65.61 ms |    2 | 481.02 KB |
| Parallel_1A   | Seattle       | 200   | 750.7 ms |   563.72 ms |  30.90 ms |    2 | 149.05 KB |
| Parallel_2    | Seattle       | 200   | 741.1 ms |   231.56 ms |  12.69 ms |    2 | 480.52 KB |
| Parallel_2A   | Seattle       | 200   | 736.2 ms |   248.52 ms |  13.62 ms |    2 | 149.48 KB |
| Parallel_3    | Seattle       | 200   | 833.0 ms | 2,928.45 ms | 160.52 ms |    2 |  90.43 KB |
|||||||||
| Sequential_1  | Seattle       | 500   | 813.9 ms | 2,531.76 ms | 138.77 ms |    2 | 162.63 KB |
| Sequential_1A | Seattle       | 500   | 745.6 ms |   290.32 ms |  15.91 ms |    2 | 162.63 KB |
| Sequential_2  | Seattle       | 500   | 838.7 ms | 1,628.97 ms |  89.29 ms |    2 |  55.34 KB |
| Parallel_1    | Seattle       | 500   | 738.7 ms |   194.59 ms |  10.67 ms |    2 |  479.8 KB |
| Parallel_1A   | Seattle       | 500   | 738.5 ms |   199.62 ms |  10.94 ms |    2 | 149.38 KB |
| Parallel_2    | Seattle       | 500   | 752.5 ms |   264.54 ms |  14.50 ms |    2 | 480.43 KB |
| Parallel_2A   | Seattle       | 500   | 825.6 ms | 2,952.62 ms | 161.84 ms |    2 | 182.55 KB |
| Parallel_3    | Seattle       | 500   | 754.8 ms |   485.20 ms |  26.60 ms |    2 |  91.32 KB |
|||||||||
| Sequential_1  | Seattle       | 1000  | 743.2 ms |   311.25 ms |  17.06 ms |    2 | 114.01 KB |
| Sequential_1A | Seattle       | 1000  | 771.8 ms |   253.99 ms |  13.92 ms |    2 | 114.66 KB |
| Sequential_2  | Seattle       | 1000  | 741.5 ms |   412.80 ms |  22.63 ms |    2 |  55.47 KB |
| Parallel_1    | Seattle       | 1000  | 744.6 ms |   240.42 ms |  13.18 ms |    2 | 489.02 KB |
| Parallel_1A   | Seattle       | 1000  | 742.3 ms |   294.87 ms |  16.16 ms |    2 | 197.37 KB |
| Parallel_2    | Seattle       | 1000  | 752.1 ms |   257.07 ms |  14.09 ms |    2 | 528.18 KB |
| Parallel_2A   | Seattle       | 1000  | 732.5 ms |    55.82 ms |   3.06 ms |    2 | 149.63 KB |
| Parallel_3    | Seattle       | 1000  | 732.7 ms |   101.03 ms |   5.54 ms |    2 |  90.64 KB |

// * Legends *
  city      : Value of the 'city' parameter
  votes     : Value of the 'votes' parameter
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  Rank      : Relative position of current benchmark mean among all benchmarks (Arabic style)
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 ms      : 1 Millisecond (0.001 sec)