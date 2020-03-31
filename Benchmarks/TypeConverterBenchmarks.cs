using System;
using System.ComponentModel;
using BenchmarkDotNet.Attributes;

namespace InstanceCreator.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class TypeConverterBenchmarks
    {
        private Type targetType;
        private Int32Converter int32Converter;

        public TypeConverterBenchmarks()
        {
            targetType = typeof(int);
            int32Converter = new Int32Converter();
        }

        [Params("1000")] 
        public string Parameter { get; set; }

        [Benchmark]
        public object ChangeType()
        {
            return System.Convert.ChangeType(this.Parameter, targetType);
        }

        [Benchmark]
        public object Converter()
        {
            return int32Converter.ConvertFromString(null, Parameter);
        }

        [Benchmark]
        public object Parse()
        {
            switch (targetType)
            {
                case Type byteType when byteType == typeof(byte):
                    return int.Parse(Parameter);
                case Type shortType when shortType == typeof(short):
                    return int.Parse(Parameter);
                case Type intType when intType == typeof(int):
                    return int.Parse(Parameter);
                case Type longType when longType == typeof(long):
                    return int.Parse(Parameter);
            }
            return null;
        }
    }
}