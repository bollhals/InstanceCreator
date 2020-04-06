``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  ShortRun : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|               Method |        Mean |        Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------- |------------:|-------------:|----------:|-------:|------:|------:|----------:|
|          Ctor_Struct |    255.4 ns |      6.12 ns |   0.34 ns | 0.0625 |     - |     - |     296 B |
| Ctor_Struct_Original | 66,648.1 ns | 12,604.05 ns | 690.87 ns | 7.8125 |     - |     - |   37048 B |
|           Ctor_Class |    265.2 ns |      3.90 ns |   0.21 ns | 0.0696 |     - |     - |     328 B |
|  Ctor_Class_Original | 68,621.9 ns | 10,560.78 ns | 578.87 ns | 7.8125 |     - |     - |   37048 B |
|          Prop_Struct |    221.0 ns |     11.13 ns |   0.61 ns | 0.0627 |     - |     - |     296 B |
|           Prop_Class |    227.8 ns |     12.83 ns |   0.70 ns | 0.0696 |     - |     - |     328 B |
|  Prop_Class_Original | 40,001.5 ns |  7,274.99 ns | 398.77 ns | 4.2114 |     - |     - |   19876 B |
