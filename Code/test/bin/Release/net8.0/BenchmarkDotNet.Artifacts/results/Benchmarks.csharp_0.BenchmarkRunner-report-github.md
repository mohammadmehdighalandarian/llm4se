```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
Intel Core i3-4130 CPU 3.40GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.100
  [Host]     : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3 [AttachedDebugger]
  DefaultJob : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3


```
| Method             | N     | Mean        | Error      | StdDev     | Median      | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
|------------------- |------ |------------:|-----------:|-----------:|------------:|------:|--------:|-----:|----------:|------------:|
| GPT                | 100   |    84.34 ns |   1.666 ns |   1.559 ns |    84.11 ns |  1.00 |    0.03 |    1 |         - |          NA |
| Claude             | 100   |    99.59 ns |   1.006 ns |   0.786 ns |    99.41 ns |  1.18 |    0.02 |    2 |         - |          NA |
| Grok               | 100   |    99.88 ns |   1.305 ns |   1.019 ns |    99.70 ns |  1.18 |    0.02 |    2 |         - |          NA |
| Gemini_LateFailure | 100   |   100.07 ns |   0.905 ns |   0.802 ns |   100.00 ns |  1.19 |    0.02 |    2 |         - |          NA |
| Gemini             | 100   |   101.22 ns |   2.074 ns |   3.168 ns |   100.58 ns |  1.20 |    0.04 |    2 |         - |          NA |
| Grok_LateFailure   | 100   |   102.95 ns |   2.097 ns |   4.423 ns |   101.05 ns |  1.22 |    0.06 |    2 |         - |          NA |
| Claude_LateFailure | 100   |   103.26 ns |   2.081 ns |   4.298 ns |   101.51 ns |  1.22 |    0.06 |    2 |         - |          NA |
| GPT_LateFailure    | 100   |   109.24 ns |   2.089 ns |   1.631 ns |   109.26 ns |  1.30 |    0.03 |    2 |         - |          NA |
|                    |       |             |            |            |             |       |         |      |           |             |
| GPT                | 1000  |   750.07 ns |  14.037 ns |  26.707 ns |   739.41 ns |  1.00 |    0.05 |    1 |         - |          NA |
| Claude             | 1000  |   874.04 ns |  16.457 ns |  16.900 ns |   868.73 ns |  1.17 |    0.05 |    2 |         - |          NA |
| Claude_LateFailure | 1000  |   879.25 ns |  15.419 ns |  12.876 ns |   875.87 ns |  1.17 |    0.04 |    2 |         - |          NA |
| Gemini             | 1000  |   879.35 ns |  17.442 ns |  16.315 ns |   876.83 ns |  1.17 |    0.05 |    2 |         - |          NA |
| Gemini_LateFailure | 1000  |   882.76 ns |  13.265 ns |  15.791 ns |   876.06 ns |  1.18 |    0.04 |    2 |         - |          NA |
| Grok_LateFailure   | 1000  |   884.33 ns |  17.634 ns |  19.600 ns |   877.89 ns |  1.18 |    0.05 |    2 |         - |          NA |
| Grok               | 1000  |   887.55 ns |  17.720 ns |  25.414 ns |   877.61 ns |  1.18 |    0.05 |    2 |         - |          NA |
| GPT_LateFailure    | 1000  |   903.44 ns |  17.523 ns |  19.477 ns |   904.88 ns |  1.21 |    0.05 |    2 |         - |          NA |
|                    |       |             |            |            |             |       |         |      |           |             |
| GPT_LateFailure    | 10000 | 7,424.45 ns | 146.530 ns | 210.149 ns | 7,360.67 ns |  1.00 |    0.05 |    1 |         - |          NA |
| GPT                | 10000 | 7,457.06 ns | 142.684 ns | 300.969 ns | 7,383.93 ns |  1.00 |    0.06 |    1 |         - |          NA |
| Grok               | 10000 | 8,600.33 ns |  77.285 ns |  64.536 ns | 8,598.80 ns |  1.16 |    0.05 |    2 |         - |          NA |
| Grok_LateFailure   | 10000 | 8,620.71 ns |  92.758 ns |  77.457 ns | 8,606.59 ns |  1.16 |    0.05 |    2 |         - |          NA |
| Claude             | 10000 | 8,740.72 ns | 163.956 ns | 303.902 ns | 8,648.83 ns |  1.17 |    0.06 |    2 |         - |          NA |
| Gemini             | 10000 | 8,784.28 ns | 155.703 ns | 218.275 ns | 8,745.22 ns |  1.18 |    0.05 |    2 |         - |          NA |
| Gemini_LateFailure | 10000 | 8,821.88 ns | 174.968 ns | 315.503 ns | 8,723.95 ns |  1.18 |    0.06 |    2 |         - |          NA |
| Claude_LateFailure | 10000 | 8,836.31 ns | 173.806 ns | 338.996 ns | 8,720.92 ns |  1.19 |    0.06 |    2 |         - |          NA |
