using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace InstanceCreator.Benchmarks.Creator
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreatorToSetFullMatchBenchmark
    {
        private const int Property1Value = 100;
        private const long Property2Value = 50;

        public CreatorToSetFullMatchBenchmark()
        {
            this.Table = new Table("Property1", "Property2");
            this.Table.AddRow(Property1Value.ToString(), Property2Value.ToString());
            this.Table.AddRow(Property1Value.ToString(), Property2Value.ToString());
            this.Table.AddRow(Property1Value.ToString(), Property2Value.ToString());
            this.Table.AddRow(Property1Value.ToString(), Property2Value.ToString());
        }

        public Table Table { get; set; }
        
        [Benchmark]
        public List<StructWithTwoPropertiesAndConstructor> Ctor_Struct()
        {
            return this.Table.ToSet<StructWithTwoPropertiesAndConstructor>();
        }

        [Benchmark]
        public List<StructWithTwoPropertiesAndConstructor> Ctor_Struct_Original()
        {
            return (List<StructWithTwoPropertiesAndConstructor>)this.Table.CreateSet<StructWithTwoPropertiesAndConstructor>();
        }

        [Benchmark]
        public List<ClassWithTwoPropertiesAndConstructor> Ctor_Class()
        {
            return this.Table.ToSet<ClassWithTwoPropertiesAndConstructor>();
        }

        [Benchmark]
        public List<ClassWithTwoPropertiesAndConstructor> Ctor_Class_Original()
        {
            return (List<ClassWithTwoPropertiesAndConstructor>)this.Table.CreateSet<ClassWithTwoPropertiesAndConstructor>();
        }
        
        [Benchmark]
        public List<StructWithTwoProperties> Prop_Struct()
        {
            return this.Table.ToSet<StructWithTwoProperties>();
        }
        
        /* Not supported
        [Benchmark]
        public List<StructWithTwoProperties> Prop_Struct_Original()
        {
            return (List<StructWithTwoProperties>)this.Table.CreateSet<StructWithTwoProperties>();
        }
        */

        [Benchmark]
        public List<ClassWithTwoProperties> Prop_Class()
        {
            return this.Table.ToSet<ClassWithTwoProperties>();
        }

        [Benchmark]
        public List<ClassWithTwoProperties> Prop_Class_Original()
        {
            return (List<ClassWithTwoProperties>)this.Table.CreateSet<ClassWithTwoProperties>();
        }
    }
}