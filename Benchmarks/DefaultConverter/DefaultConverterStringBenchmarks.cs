using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DefaultConverterStringBenchmarks
    {
        public DefaultConverterStringBenchmarks()
        {
            this.StringValue = "ABC";
        }

        public string StringValue { get; set; }

        [Benchmark]
        public string Convert()
        {
            TypeConverter<string>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}