using BenchmarkDotNet.Attributes;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;

namespace InstanceCreator.Benchmarks.Creator
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreatorInstanceInInstanceBenchmark
    {
        private const int Property1Value = 100;
        private const long Property2Value = 50;

        public CreatorInstanceInInstanceBenchmark()
        {
            this.Table = new Table("Property1", "Property2", "Inner.Property1", "Inner.Property2");
            this.Table.AddRow(Property1Value.ToString(), Property2Value.ToString(), Property1Value.ToString(), Property2Value.ToString());
        }

        public Table Table { get; set; }
        
        [Benchmark]
        public StructInStructWithConstructor Ctor_Struct()
        {
            return this.Table.ToInstance<StructInStructWithConstructor>();
        }
        
        [Benchmark]
        public ClassInClassWithConstructor Ctor_Class()
        {
            return this.Table.ToInstance<ClassInClassWithConstructor>();
        }
        
        [Benchmark]
        public StructInStruct Prop_Struct()
        {
            return this.Table.ToInstance<StructInStruct>();
        }
        
        [Benchmark]
        public ClassInClass Prop_Class()
        {
            return this.Table.ToInstance<ClassInClass>();
        }

        /* Specflow does not support instance in instance */
    }
}