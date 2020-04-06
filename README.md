# InstanceCreator

A faster version of CreateInstance & CreateSet of Specflow (see https://specflow.org/documentation/SpecFlow-Assist-Helpers/)

For details, see example benchmarks in [docs](docs/benchmarks).

Most relevant ones are 
[CreatorToSetFullMatchBenchmark](docs/benchmarks/InstanceCreator.Benchmarks.Creator.CreatorToSetFullMatchBenchmark-report-github.md)(Creating a instance with 2 properties)
|               Method |        Mean |       Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------- |------------:|------------:|----------:|-------:|------:|------:|----------:|
|          Ctor_Struct |    173.4 ns |     3.95 ns |   0.22 ns | 0.0083 |     - |     - |      40 B |
| Ctor_Struct_Original | 82,231.6 ns | 7,501.73 ns | 411.20 ns | 9.8877 |     - |     - |   46618 B |
|           Ctor_Class |    180.2 ns |     1.58 ns |   0.09 ns | 0.0153 |     - |     - |      72 B |
|  Ctor_Class_Original | 83,246.2 ns | 9,303.59 ns | 509.96 ns | 9.8877 |     - |     - |   46618 B |
|          Prop_Struct |    143.9 ns |     4.81 ns |   0.26 ns | 0.0083 |     - |     - |      40 B |
|           Prop_Class |    149.1 ns |    11.50 ns |   0.63 ns | 0.0153 |     - |     - |      72 B |
|  Prop_Class_Original | 56,505.8 ns | 3,558.58 ns | 195.06 ns | 6.2256 |     - |     - |   29446 B |

and [CreatorToSetFullMatchBenchmark](docs/benchmarks/InstanceCreator.Benchmarks.Creator.CreatorToSetFullMatchBenchmark-report-github.md) (Creating 4 instances with 2 properties).
|               Method |         Mean |        Error |      StdDev |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------- |-------------:|-------------:|------------:|--------:|------:|------:|----------:|
|          Ctor_Struct |     455.6 ns |     28.33 ns |     1.55 ns |  0.0591 |     - |     - |     280 B |
| Ctor_Struct_Original | 281,236.7 ns | 27,270.11 ns | 1,494.77 ns | 31.7383 |     - |     - |  150136 B |
|           Ctor_Class |     488.6 ns |     54.83 ns |     3.01 ns |  0.0792 |     - |     - |     376 B |
|  Ctor_Class_Original | 283,926.8 ns | 27,748.67 ns | 1,521.00 ns | 31.7383 |     - |     - |  150104 B |
|          Prop_Struct |     424.0 ns |      0.92 ns |     0.05 ns |  0.0591 |     - |     - |     280 B |
|           Prop_Class |     439.5 ns |     20.05 ns |     1.10 ns |  0.0796 |     - |     - |     376 B |
|  Prop_Class_Original | 164,573.2 ns | 27,688.43 ns | 1,517.70 ns | 17.0898 |     - |     - |   81417 B |
