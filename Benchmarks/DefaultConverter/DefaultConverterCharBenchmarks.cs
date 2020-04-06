using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DefaultConverterCharBenchmarks
    {
        public DefaultConverterCharBenchmarks()
        {
            this.StringValue = "A";
        }

        public string StringValue { get; set; }

        [Benchmark]
        public char Convert()
        {
            TypeConverter<char>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
        
        [Benchmark]
        public char? Convert_Nullable()
        {
            TypeConverter<char?>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}