using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DefaultConverterBoolBenchmarks
    {
        public DefaultConverterBoolBenchmarks()
        {
            this.StringValue = "True";
        }

        public string StringValue { get; set; }

        [Benchmark]
        public bool Convert()
        {
            TypeConverter<bool>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
        
        [Benchmark]
        public bool? Convert_Nullable()
        {
            TypeConverter<bool?>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}