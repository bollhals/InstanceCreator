using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;
using InstanceCreator.Tests.Types;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DefaultConverterFlagsBenchmarks
    {
        public DefaultConverterFlagsBenchmarks()
        {
            this.StringValue = "A, B";
        }

        public string StringValue { get; set; }

        [Benchmark]
        public OptionFlags Convert()
        {
            TypeConverter<OptionFlags>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
        
        [Benchmark]
        public OptionFlags? Convert_Nullable()
        {
            TypeConverter<OptionFlags?>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}