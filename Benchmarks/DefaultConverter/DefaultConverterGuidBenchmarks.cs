using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DefaultConverterGuidBenchmarks
    {
        public DefaultConverterGuidBenchmarks()
        {
            this.StringValue = "{781BF165-7A9A-41F8-A930-7909A7C145B8}";
        }

        public string StringValue { get; set; }

        [Benchmark]
        public Guid Convert()
        {
            TypeConverter<Guid>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
        
        [Benchmark]
        public Guid? Convert_Nullable()
        {
            TypeConverter<Guid?>.Convert(StringValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}