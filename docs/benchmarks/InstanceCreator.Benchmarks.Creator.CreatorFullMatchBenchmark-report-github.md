``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  ShortRun : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|               Method |        Mean |       Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------- |------------:|------------:|----------:|-------:|------:|------:|----------:|
|          Ctor_Struct |    173.4 ns |     3.95 ns |   0.22 ns | 0.0083 |     - |     - |      40 B |
| Ctor_Struct_Original | 82,231.6 ns | 7,501.73 ns | 411.20 ns | 9.8877 |     - |     - |   46618 B |
|           Ctor_Class |    180.2 ns |     1.58 ns |   0.09 ns | 0.0153 |     - |     - |      72 B |
|  Ctor_Class_Original | 83,246.2 ns | 9,303.59 ns | 509.96 ns | 9.8877 |     - |     - |   46618 B |
|          Prop_Struct |    143.9 ns |     4.81 ns |   0.26 ns | 0.0083 |     - |     - |      40 B |
|           Prop_Class |    149.1 ns |    11.50 ns |   0.63 ns | 0.0153 |     - |     - |      72 B |
|  Prop_Class_Original | 56,505.8 ns | 3,558.58 ns | 195.06 ns | 6.2256 |     - |     - |   29446 B |
