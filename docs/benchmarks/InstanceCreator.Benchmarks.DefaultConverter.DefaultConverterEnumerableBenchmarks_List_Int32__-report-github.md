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
| **Convert** |              **** |  **16.14 ns** | **1.343 ns** | **0.074 ns** | **0.0068** |     **-** |     **-** |      **32 B** |
| **Convert** | **1, 2, 3, 4, 5** | **187.03 ns** | **6.961 ns** | **0.382 ns** | **0.0627** |     **-** |     **-** |     **296 B** |
| **Convert** |   **wrong value** |  **61.83 ns** | **6.313 ns** | **0.346 ns** | **0.0204** |     **-** |     **-** |      **96 B** |
