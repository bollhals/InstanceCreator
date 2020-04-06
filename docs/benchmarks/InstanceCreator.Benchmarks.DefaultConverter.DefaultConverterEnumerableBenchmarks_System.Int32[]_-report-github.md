``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  ShortRun : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|  Method |   StringValue |      Mean |     Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------- |-------------- |----------:|----------:|---------:|-------:|------:|------:|----------:|
| **Convert** |              **** |  **10.20 ns** |  **0.419 ns** | **0.023 ns** |      **-** |     **-** |     **-** |         **-** |
| **Convert** | **1, 2, 3, 4, 5** | **178.46 ns** | **26.706 ns** | **1.464 ns** | **0.0560** |     **-** |     **-** |     **264 B** |
| **Convert** |   **wrong value** |  **51.84 ns** |  **8.182 ns** | **0.448 ns** | **0.0136** |     **-** |     **-** |      **64 B** |
