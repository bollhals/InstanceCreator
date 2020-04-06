``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  ShortRun : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|               Method |         Mean |        Error |      StdDev |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------- |-------------:|-------------:|------------:|--------:|------:|------:|----------:|
|          Ctor_Struct |     455.6 ns |     28.33 ns |     1.55 ns |  0.0591 |     - |     - |     280 B |
| Ctor_Struct_Original | 281,236.7 ns | 27,270.11 ns | 1,494.77 ns | 31.7383 |     - |     - |  150136 B |
|           Ctor_Class |     488.6 ns |     54.83 ns |     3.01 ns |  0.0792 |     - |     - |     376 B |
|  Ctor_Class_Original | 283,926.8 ns | 27,748.67 ns | 1,521.00 ns | 31.7383 |     - |     - |  150104 B |
|          Prop_Struct |     424.0 ns |      0.92 ns |     0.05 ns |  0.0591 |     - |     - |     280 B |
|           Prop_Class |     439.5 ns |     20.05 ns |     1.10 ns |  0.0796 |     - |     - |     376 B |
|  Prop_Class_Original | 164,573.2 ns | 27,688.43 ns | 1,517.70 ns | 17.0898 |     - |     - |   81417 B |
