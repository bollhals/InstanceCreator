using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DefaultConverterDateTimeOffsetBenchmarks
    {
        public DefaultConverterDateTimeOffsetBenchmarks()
        {
            this.StringValue = "2020-03-05T06:05:04.003+02:00";
        }

        public string StringValue { get; set; }

        [Benchmark]
        public DateTimeOffset Convert()
        {
            TypeConverter<DateTimeOffset>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
        
        [Benchmark]
        public DateTimeOffset? Convert_Nullable()
        {
            TypeConverter<DateTimeOffset?>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}