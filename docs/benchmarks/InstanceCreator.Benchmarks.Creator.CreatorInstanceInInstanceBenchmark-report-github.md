``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  ShortRun : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|      Method |     Mean |     Error |  StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------ |---------:|----------:|--------:|-------:|------:|------:|----------:|
| Ctor_Struct | 618.6 ns |  24.90 ns | 1.36 ns | 0.1249 |     - |     - |     592 B |
|  Ctor_Class | 599.0 ns |  11.32 ns | 0.62 ns | 0.1402 |     - |     - |     664 B |
| Prop_Struct | 530.2 ns | 139.19 ns | 7.63 ns | 0.1249 |     - |     - |     592 B |
|  Prop_Class | 530.6 ns |   9.99 ns | 0.55 ns | 0.1402 |     - |     - |     664 B |