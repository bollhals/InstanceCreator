``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  ShortRun : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|  Method |   StringValue |      Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------- |-------------- |----------:|---------:|---------:|-------:|------:|------:|----------:|
| **Convert** |              **** |  **13.22 ns** | **1.040 ns** | **0.057 ns** |      **-** |     **-** |     **-** |         **-** |
| **Convert** | **1, 2, 3, 4, 5** | **182.30 ns** | **5.420 ns** | **0.297 ns** | **0.0560** |     **-** |     **-** |     **264 B** |
| **Convert** |   **wrong value** |  **55.84 ns** | **5.010 ns** | **0.275 ns** | **0.0136** |     **-** |     **-** |      **64 B** |
