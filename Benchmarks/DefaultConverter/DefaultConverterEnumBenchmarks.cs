using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;
using InstanceCreator.Tests.Types;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DefaultConverterEnumBenchmarks
    {
        public DefaultConverterEnumBenchmarks()
        {
            this.StringValue = "Red";
        }

        public string StringValue { get; set; }

        [Benchmark]
        public ColorEnum Convert()
        {
            TypeConverter<ColorEnum>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
        
        [Benchmark]
        public ColorEnum? Convert_Nullable()
        {
            TypeConverter<ColorEnum?>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}