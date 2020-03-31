using System;
using System.ComponentModel;
using BenchmarkDotNet.Attributes;

namespace InstanceCreator.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class IsPrimiviteBenchmarks
    {
        [Params(typeof(decimal), typeof(int), typeof(int?))] 
        public Type Parameter { get; set; }

        [Benchmark]
        public bool IsPrimitive()
        {
            return this.Parameter.IsPrimitive;
        }

        [Benchmark]
        public bool Converter()
        {
            return typeof(IConvertible).IsAssignableFrom(this.Parameter);
        }

        [Benchmark]
        public bool NullablePrimitive()
        {
            var underlyingType = Nullable.GetUnderlyingType(this.Parameter);
            return underlyingType != null && underlyingType.IsPrimitive;
        }
    }
}