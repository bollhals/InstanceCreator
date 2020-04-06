``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  ShortRun : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|      Method |     Mean |    Error |    StdDev |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
|------------ |---------:|---------:|----------:|-------:|-------:|-------:|----------:|
| Ctor_Struct | 4.472 μs | 1.104 μs | 0.0605 μs | 0.3510 | 0.1678 | 0.0229 |   1.62 KB |
|  Ctor_Class | 4.307 μs | 2.038 μs | 0.1117 μs | 0.3510 | 0.1678 | 0.0153 |   1.62 KB |
| Prop_Struct | 4.590 μs | 1.647 μs | 0.0903 μs | 0.3662 | 0.1755 | 0.0305 |    1.7 KB |
|  Prop_Class | 4.979 μs | 2.970 μs | 0.1628 μs | 0.4044 | 0.1907 | 0.0305 |   1.87 KB |
