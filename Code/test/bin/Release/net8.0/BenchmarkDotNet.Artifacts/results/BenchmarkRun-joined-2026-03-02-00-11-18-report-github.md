```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
Intel Core i3-4130 CPU 3.40GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.100
  [Host]     : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3 [AttachedDebugger]
  DefaultJob : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3


```
| Namespace            | Method | N    | Mean           | Error         | StdDev        | Ratio      | RatioSD  | Rank | Gen0    | Gen1    | Allocated | Alloc Ratio |
|--------------------- |------- |----- |---------------:|--------------:|--------------:|-----------:|---------:|-----:|--------:|--------:|----------:|------------:|
| Benchmarks.csharp_24 | Grok   | 100  |      0.3188 ns |     0.0123 ns |     0.0096 ns |       0.34 |     0.01 |    1 |       - |       - |         - |          NA |
| Benchmarks.csharp_24 | Claude | 100  |      0.3221 ns |     0.0151 ns |     0.0141 ns |       0.34 |     0.02 |    1 |       - |       - |         - |          NA |
| Benchmarks.csharp_24 | Gemini | 100  |      0.3264 ns |     0.0165 ns |     0.0129 ns |       0.35 |     0.02 |    1 |       - |       - |         - |          NA |
| Benchmarks.csharp_24 | GPT    | 100  |      0.9392 ns |     0.0251 ns |     0.0210 ns |       1.00 |     0.03 |    2 |       - |       - |         - |          NA |
| Benchmarks.csharp_21 | Grok   | 100  |     68.0426 ns |     0.5445 ns |     0.4827 ns |      72.48 |     1.60 |    3 |       - |       - |         - |          NA |
| Benchmarks.csharp_21 | Claude | 100  |     68.5952 ns |     1.1551 ns |     0.9646 ns |      73.07 |     1.82 |    3 |       - |       - |         - |          NA |
| Benchmarks.csharp_21 | Gemini | 100  |     68.9108 ns |     0.4836 ns |     0.4287 ns |      73.40 |     1.60 |    3 |       - |       - |         - |          NA |
| Benchmarks.csharp_21 | GPT    | 100  |     69.1215 ns |     0.3840 ns |     0.3206 ns |      73.63 |     1.58 |    3 |       - |       - |         - |          NA |
| Benchmarks.csharp_26 | GPT    | 100  |     75.3564 ns |     0.4469 ns |     0.3732 ns |      80.27 |     1.73 |    4 |       - |       - |         - |          NA |
| Benchmarks.csharp_26 | Gemini | 100  |     75.7413 ns |     0.7706 ns |     0.6831 ns |      80.68 |     1.83 |    4 |       - |       - |         - |          NA |
| Benchmarks.csharp_26 | Claude | 100  |     76.1667 ns |     0.7687 ns |     0.7190 ns |      81.13 |     1.86 |    4 |       - |       - |         - |          NA |
| Benchmarks.csharp_26 | Grok   | 100  |     76.1927 ns |     0.5132 ns |     0.4006 ns |      81.16 |     1.75 |    4 |       - |       - |         - |          NA |
| Benchmarks.csharp_30 | Grok   | 100  |    136.4474 ns |     0.7289 ns |     0.6818 ns |     145.35 |     3.13 |    5 |       - |       - |         - |          NA |
| Benchmarks.csharp_30 | Gemini | 100  |    154.6665 ns |     0.8010 ns |     0.7493 ns |     164.75 |     3.54 |    6 |       - |       - |         - |          NA |
| Benchmarks.csharp_30 | GPT    | 100  |    165.1645 ns |     1.6469 ns |     1.4599 ns |     175.94 |     3.98 |    7 |       - |       - |         - |          NA |
| Benchmarks.csharp_27 | GPT    | 100  |    171.7811 ns |     1.5641 ns |     1.4631 ns |     182.98 |     4.12 |    8 |       - |       - |         - |          NA |
| Benchmarks.csharp_27 | Gemini | 100  |    193.6085 ns |     1.6930 ns |     1.4137 ns |     206.23 |     4.56 |    9 |       - |       - |         - |          NA |
| Benchmarks.csharp_25 | GPT    | 100  |    262.4702 ns |     4.5057 ns |     4.8211 ns |     279.59 |     7.70 |   10 |  0.2904 |       - |     456 B |          NA |
| Benchmarks.csharp_30 | Claude | 100  |    305.2808 ns |     2.7169 ns |     2.5414 ns |     325.19 |     7.30 |   11 |  0.3209 |       - |     504 B |          NA |
| Benchmarks.csharp_25 | Grok   | 100  |    406.3895 ns |     4.2353 ns |     4.1597 ns |     432.89 |    10.04 |   12 |  0.7548 |       - |    1184 B |          NA |
| Benchmarks.csharp_25 | Gemini | 100  |    408.9405 ns |     5.6012 ns |     5.2394 ns |     435.61 |    10.61 |   12 |  0.7548 |       - |    1184 B |          NA |
| Benchmarks.csharp_23 | Grok   | 100  |    494.2640 ns |     2.9723 ns |     2.6349 ns |     526.50 |    11.37 |   13 |       - |       - |         - |          NA |
| Benchmarks.csharp_23 | GPT    | 100  |    495.0589 ns |     2.9002 ns |     2.7129 ns |     527.34 |    11.40 |   13 |       - |       - |         - |          NA |
| Benchmarks.csharp_23 | Gemini | 100  |    495.9006 ns |     3.4291 ns |     2.8634 ns |     528.24 |    11.46 |   13 |       - |       - |         - |          NA |
| Benchmarks.csharp_27 | Grok   | 100  |    510.3171 ns |     3.4151 ns |     2.8518 ns |     543.60 |    11.77 |   13 |       - |       - |         - |          NA |
| Benchmarks.csharp_27 | Claude | 100  |    558.3380 ns |     4.0445 ns |     3.5854 ns |     594.75 |    13.00 |   14 |  0.0200 |       - |      32 B |          NA |
| Benchmarks.csharp_23 | Claude | 100  |    975.4758 ns |     7.4862 ns |     7.0026 ns |   1,039.09 |    22.95 |   15 |       - |       - |         - |          NA |
| Benchmarks.csharp_25 | Claude | 100  |  1,306.5575 ns |    16.3413 ns |    14.4861 ns |   1,391.76 |    32.77 |   16 |  0.8297 |       - |    1304 B |          NA |
| Benchmarks.csharp_29 | GPT    | 100  |  1,453.5710 ns |    23.6191 ns |    28.1169 ns |   1,548.37 |    43.70 |   17 |  1.5297 |       - |    2400 B |          NA |
| Benchmarks.csharp_29 | Gemini | 100  |  1,478.3939 ns |    13.6118 ns |    11.3665 ns |   1,574.81 |    35.02 |   17 |  1.5297 |       - |    2400 B |          NA |
| Benchmarks.csharp_29 | Claude | 100  |  1,481.2175 ns |     9.8420 ns |     8.2185 ns |   1,577.82 |    34.14 |   17 |  1.5297 |       - |    2400 B |          NA |
| Benchmarks.csharp_29 | Grok   | 100  |  1,485.1319 ns |     9.4466 ns |     7.3752 ns |   1,581.99 |    34.03 |   17 |  1.5297 |       - |    2400 B |          NA |
| Benchmarks.csharp_28 | GPT    | 100  |  2,654.3916 ns |    38.9357 ns |    34.5155 ns |   2,827.50 |    69.11 |   18 |  4.7073 |       - |    7392 B |          NA |
| Benchmarks.csharp_22 | Grok   | 100  |  4,281.6296 ns |    34.3270 ns |    32.1095 ns |   4,560.86 |   101.18 |   19 |  2.9526 |       - |    4640 B |          NA |
| Benchmarks.csharp_22 | Claude | 100  |  4,350.8378 ns |    83.0149 ns |    69.3212 ns |   4,634.58 |   120.46 |   19 |  2.9526 |       - |    4640 B |          NA |
| Benchmarks.csharp_22 | Gemini | 100  |  4,373.7486 ns |    83.2419 ns |    73.7917 ns |   4,658.98 |   123.74 |   19 |  2.9526 |       - |    4640 B |          NA |
| Benchmarks.csharp_22 | GPT    | 100  |  4,454.7741 ns |    87.4793 ns |    93.6018 ns |   4,745.29 |   138.99 |   19 |  5.3024 |       - |    8328 B |          NA |
| Benchmarks.csharp_28 | Gemini | 100  |  6,150.4237 ns |    63.5433 ns |    59.4384 ns |   6,551.52 |   150.40 |   20 |  7.1106 |       - |   11160 B |          NA |
| Benchmarks.csharp_28 | Claude | 100  |  8,885.3218 ns |    95.7382 ns |    74.7461 ns |   9,464.78 |   212.75 |   21 | 11.8866 |       - |   18648 B |          NA |
| Benchmarks.csharp_28 | Grok   | 100  |  8,918.9813 ns |    98.4116 ns |    87.2393 ns |   9,500.63 |   218.49 |   21 | 11.8561 |       - |   18600 B |          NA |
|                      |        |      |                |               |               |            |          |      |         |         |           |             |
| Benchmarks.csharp_24 | Claude | 1000 |      0.3152 ns |     0.0224 ns |     0.0175 ns |       0.34 |     0.02 |    1 |       - |       - |         - |          NA |
| Benchmarks.csharp_24 | Gemini | 1000 |      0.3202 ns |     0.0109 ns |     0.0097 ns |       0.35 |     0.01 |    1 |       - |       - |         - |          NA |
| Benchmarks.csharp_24 | Grok   | 1000 |      0.3285 ns |     0.0354 ns |     0.0296 ns |       0.36 |     0.03 |    1 |       - |       - |         - |          NA |
| Benchmarks.csharp_24 | GPT    | 1000 |      0.9190 ns |     0.0164 ns |     0.0146 ns |       1.00 |     0.02 |    2 |       - |       - |         - |          NA |
| Benchmarks.csharp_21 | Grok   | 1000 |    617.8912 ns |     2.1002 ns |     1.6397 ns |     672.49 |    10.31 |    3 |       - |       - |         - |          NA |
| Benchmarks.csharp_21 | GPT    | 1000 |    619.1586 ns |     2.8251 ns |     2.5044 ns |     673.87 |    10.52 |    3 |       - |       - |         - |          NA |
| Benchmarks.csharp_21 | Gemini | 1000 |    619.8713 ns |     4.0933 ns |     3.6286 ns |     674.64 |    10.88 |    3 |       - |       - |         - |          NA |
| Benchmarks.csharp_21 | Claude | 1000 |    620.5202 ns |     4.2797 ns |     4.0032 ns |     675.35 |    11.04 |    3 |       - |       - |         - |          NA |
| Benchmarks.csharp_26 | Gemini | 1000 |    716.2746 ns |     5.4257 ns |     4.8098 ns |     779.57 |    12.82 |    4 |       - |       - |         - |          NA |
| Benchmarks.csharp_26 | GPT    | 1000 |    716.9939 ns |     6.9160 ns |     6.1309 ns |     780.35 |    13.44 |    4 |       - |       - |         - |          NA |
| Benchmarks.csharp_26 | Grok   | 1000 |    717.4572 ns |     7.7325 ns |     6.4570 ns |     780.85 |    13.61 |    4 |       - |       - |         - |          NA |
| Benchmarks.csharp_26 | Claude | 1000 |    717.6249 ns |     6.5360 ns |     5.4579 ns |     781.03 |    13.12 |    4 |       - |       - |         - |          NA |
| Benchmarks.csharp_30 | Grok   | 1000 |  1,205.3525 ns |     6.3907 ns |     5.9778 ns |   1,311.86 |    20.80 |    5 |       - |       - |         - |          NA |
| Benchmarks.csharp_30 | Gemini | 1000 |  1,368.5541 ns |     5.8934 ns |     5.5127 ns |   1,489.48 |    23.24 |    6 |       - |       - |         - |          NA |
| Benchmarks.csharp_27 | GPT    | 1000 |  1,446.4197 ns |     9.3528 ns |     8.2910 ns |   1,574.23 |    25.33 |    7 |       - |       - |         - |          NA |
| Benchmarks.csharp_27 | Gemini | 1000 |  1,495.4458 ns |    16.4619 ns |    13.7464 ns |   1,627.58 |    28.51 |    7 |       - |       - |         - |          NA |
| Benchmarks.csharp_30 | GPT    | 1000 |  1,528.7061 ns |     6.4388 ns |     5.7079 ns |   1,663.78 |    25.85 |    7 |       - |       - |         - |          NA |
| Benchmarks.csharp_30 | Claude | 1000 |  2,306.5213 ns |    39.9950 ns |    42.7942 ns |   2,510.33 |    59.11 |    8 |  2.6169 |       - |    4104 B |          NA |
| Benchmarks.csharp_25 | GPT    | 1000 |  2,396.9161 ns |    36.1122 ns |    41.5869 ns |   2,608.71 |    59.20 |    8 |  2.5826 |       - |    4056 B |          NA |
| Benchmarks.csharp_25 | Gemini | 1000 |  3,064.0353 ns |    22.9065 ns |    19.1280 ns |   3,334.78 |    54.24 |    9 |  5.3749 |       - |    8424 B |          NA |
| Benchmarks.csharp_25 | Grok   | 1000 |  3,068.5732 ns |    36.3936 ns |    32.2619 ns |   3,339.71 |    60.81 |    9 |  5.3749 |       - |    8424 B |          NA |
| Benchmarks.csharp_27 | Grok   | 1000 |  4,736.6452 ns |    22.8269 ns |    19.0615 ns |   5,155.18 |    80.43 |   10 |       - |       - |         - |          NA |
| Benchmarks.csharp_23 | GPT    | 1000 |  4,822.9955 ns |    23.2138 ns |    20.5784 ns |   5,249.16 |    82.21 |   10 |       - |       - |         - |          NA |
| Benchmarks.csharp_23 | Gemini | 1000 |  4,828.9540 ns |    24.7051 ns |    23.1092 ns |   5,255.64 |    83.05 |   10 |       - |       - |         - |          NA |
| Benchmarks.csharp_23 | Grok   | 1000 |  4,841.3091 ns |    47.7218 ns |    44.6390 ns |   5,269.09 |    92.47 |   10 |       - |       - |         - |          NA |
| Benchmarks.csharp_27 | Claude | 1000 |  5,102.0459 ns |    37.9202 ns |    35.4706 ns |   5,552.87 |    91.85 |   11 |  0.0153 |       - |      32 B |          NA |
| Benchmarks.csharp_23 | Claude | 1000 |  9,674.9791 ns |    72.6758 ns |    64.4252 ns |  10,529.87 |   172.93 |   12 |       - |       - |         - |          NA |
| Benchmarks.csharp_25 | Claude | 1000 | 10,870.2000 ns |   114.6524 ns |   101.6364 ns |  11,830.70 |   208.28 |   13 |  5.4321 |       - |    8544 B |          NA |
| Benchmarks.csharp_29 | Claude | 1000 | 14,755.8587 ns |   139.9649 ns |   124.0752 ns |  16,059.70 |   275.52 |   14 | 15.2893 |       - |   24000 B |          NA |
| Benchmarks.csharp_29 | Grok   | 1000 | 14,757.3790 ns |   135.3633 ns |   126.6189 ns |  16,061.35 |   276.93 |   14 | 15.2893 |       - |   24000 B |          NA |
| Benchmarks.csharp_29 | Gemini | 1000 | 14,783.1709 ns |   143.7604 ns |   127.4399 ns |  16,089.42 |   277.60 |   14 | 15.2893 |       - |   24000 B |          NA |
| Benchmarks.csharp_29 | GPT    | 1000 | 17,015.6681 ns |   134.0831 ns |   125.4214 ns |  18,519.19 |   309.44 |   15 | 15.2893 |       - |   24000 B |          NA |
| Benchmarks.csharp_28 | GPT    | 1000 | 25,070.0915 ns |   302.3606 ns |   268.0348 ns |  27,285.30 |   499.42 |   16 | 46.5088 |       - |   73168 B |          NA |
| Benchmarks.csharp_22 | GPT    | 1000 | 44,086.7295 ns |   699.5572 ns |   654.3663 ns |  47,982.27 | 1,000.62 |   17 | 51.2695 |       - |   80528 B |          NA |
| Benchmarks.csharp_22 | Claude | 1000 | 44,771.5449 ns |   461.1174 ns |   385.0539 ns |  48,727.59 |   839.87 |   17 | 24.4751 |       - |   38616 B |          NA |
| Benchmarks.csharp_22 | Gemini | 1000 | 45,665.5371 ns |   564.9756 ns |   528.4785 ns |  49,700.58 |   934.94 |   17 | 24.4751 |       - |   38616 B |          NA |
| Benchmarks.csharp_22 | Grok   | 1000 | 45,696.4348 ns |   331.5826 ns |   276.8865 ns |  49,734.21 |   805.78 |   17 | 24.4751 |       - |   38616 B |          NA |
| Benchmarks.csharp_28 | Gemini | 1000 | 62,121.7751 ns | 1,241.7625 ns | 1,933.2747 ns |  67,610.91 | 2,310.88 |   18 | 59.5703 |  8.0566 |  104744 B |          NA |
| Benchmarks.csharp_28 | Claude | 1000 | 92,942.7584 ns | 1,769.1135 ns | 1,477.2899 ns | 101,155.26 | 2,176.49 |   19 | 78.3691 | 19.5313 |  178008 B |          NA |
| Benchmarks.csharp_28 | Grok   | 1000 | 93,240.5552 ns |   802.2518 ns |   711.1753 ns | 101,479.37 | 1,706.02 |   19 | 77.8809 | 19.4092 |  177960 B |          NA |
