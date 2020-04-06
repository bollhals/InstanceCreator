``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  ShortRun : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                      Method |         Mean |        Error |     StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |-------------:|-------------:|-----------:|-------:|------:|------:|----------:|
|             Prop_Class_Full |    150.33 ns |     7.239 ns |   0.397 ns | 0.0153 |     - |     - |      72 B |
|    Prop_Class_Full_Original | 56,225.99 ns | 3,812.740 ns | 208.989 ns | 6.2256 |     - |     - |   29446 B |
|          Prop_Class_Partial |    118.00 ns |     3.282 ns |   0.180 ns | 0.0153 |     - |     - |      72 B |
| Prop_Class_Partial_Original | 22,408.98 ns | 2,528.710 ns | 138.607 ns | 2.3193 |     - |     - |   11002 B |
|               Prop_Class_No |     88.81 ns |     1.209 ns |   0.066 ns | 0.0153 |     - |     - |      72 B |
|      Prop_Class_No_Original | 19,523.60 ns | 1,656.966 ns |  90.824 ns | 2.2278 |     - |     - |   10618 B |
