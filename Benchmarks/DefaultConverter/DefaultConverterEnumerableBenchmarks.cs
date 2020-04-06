using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    [GenericTypeArguments(typeof(int[]))]
    [GenericTypeArguments(typeof(IEnumerable<int>))]
    [GenericTypeArguments(typeof(ICollection<int>))]
    [GenericTypeArguments(typeof(IReadOnlyCollection<int>))]
    [GenericTypeArguments(typeof(IReadOnlyList<int>))]
    [GenericTypeArguments(typeof(Collection<int>))]
    [GenericTypeArguments(typeof(List<int>))]
    public class DefaultConverterEnumerableBenchmarks<T>
    {
        [Params("1, 2, 3, 4, 5", "", "wrong value")]
        public string StringValue { get; set; }

        [Benchmark]
        public void Convert()
        {
            TypeConverter<T>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
        }
    }
}