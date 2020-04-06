using System.Globalization;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Converter;

namespace InstanceCreator.Benchmarks.DefaultConverter
{
    [ShortRunJob]
    [MemoryDiagnoser]
    [GenericTypeArguments(typeof(byte))]
    [GenericTypeArguments(typeof(sbyte))]
    [GenericTypeArguments(typeof(short))]
    [GenericTypeArguments(typeof(ushort))]
    [GenericTypeArguments(typeof(int))]
    [GenericTypeArguments(typeof(uint))]
    [GenericTypeArguments(typeof(long))]
    [GenericTypeArguments(typeof(ulong))]
    public class DefaultConverterIntegerBenchmarks<T> where T : struct
    {
        public DefaultConverterIntegerBenchmarks()
        {
            this.IntegerValue = "50";
        }

        public string IntegerValue { get; set; }

        [Benchmark]
        public T Convert()
        {
            TypeConverter<T>.Convert(IntegerValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
        
        [Benchmark]
        public T? Convert_Nullable()
        {
            TypeConverter<T?>.Convert(IntegerValue, CultureInfo.InvariantCulture, out var value);
            return value;
        }
    }
}