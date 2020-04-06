using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    [GenericTypeArguments(typeof(float))]
    [GenericTypeArguments(typeof(double))]
    [GenericTypeArguments(typeof(decimal))]
    public class DefaultConverterFloatingPointBenchmarks<T> where T : struct
    {
        public DefaultConverterFloatingPointBenchmarks()
        {
            this.FloatingPointValue = "50.556";
        }

        public string FloatingPointValue { get; set; }

        [Benchmark]
        public T Convert()
        {
            TypeConverter<T>.Convert(FloatingPointValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }

        [Benchmark]
        public T? Convert_Nullable()
        {
            TypeConverter<T?>.Convert(FloatingPointValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}