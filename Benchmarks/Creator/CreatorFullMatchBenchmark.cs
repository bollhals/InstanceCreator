﻿using BenchmarkDotNet.Attributes;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace InstanceCreator.Benchmarks.Creator
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreatorFullMatchBenchmark
    {
        private const int Property1Value = 100;
        private const long Property2Value = 50;

        public CreatorFullMatchBenchmark()
        {
            this.Table = new Table("Property1", "Property2");
            this.Table.AddRow(Property1Value.ToString(), Property2Value.ToString());
        }

        public Table Table { get; set; }
        
        [Benchmark]
        public StructWithTwoPropertiesAndConstructor Ctor_Struct()
        {
            return this.Table.ToInstance<StructWithTwoPropertiesAndConstructor>();
        }

        [Benchmark]
        public StructWithTwoPropertiesAndConstructor Ctor_Struct_Original()
        {
            return this.Table.CreateInstance<StructWithTwoPropertiesAndConstructor>();
        }

        [Benchmark]
        public ClassWithTwoPropertiesAndConstructor Ctor_Class()
        {
            return this.Table.ToInstance<ClassWithTwoPropertiesAndConstructor>();
        }

        [Benchmark]
        public ClassWithTwoPropertiesAndConstructor Ctor_Class_Original()
        {
            return this.Table.CreateInstance<ClassWithTwoPropertiesAndConstructor>();
        }

        [Benchmark]
        public StructWithTwoProperties Prop_Struct()
        {
            return this.Table.ToInstance<StructWithTwoProperties>();
        }

        /* Not supported
        [Benchmark]
        public StructWithTwoProperties Prop_Struct_Original()
        {
            return this.Table.CreateInstance<StructWithTwoProperties>();
        }
        */

        [Benchmark]
        public ClassWithTwoProperties Prop_Class()
        {
            return this.Table.ToInstance<ClassWithTwoProperties>();
        }

        [Benchmark]
        public ClassWithTwoProperties Prop_Class_Original()
        {
            return this.Table.CreateInstance<ClassWithTwoProperties>();
        }
    }
}