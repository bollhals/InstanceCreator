using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DefaultConverterDateTimeBenchmarks
    {
        public DefaultConverterDateTimeBenchmarks()
        {
            this.StringValue = "2020-03-05T06:05:04.003";
        }

        public string StringValue { get; set; }

        [Benchmark]
        public DateTime Convert()
        {
            TypeConverter<DateTime>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
        
        [Benchmark]
        public DateTime? Convert_Nullable()
        {
            TypeConverter<DateTime?>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}