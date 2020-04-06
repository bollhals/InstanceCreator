using BenchmarkDotNet.Attributes;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;

namespace InstanceCreator.Benchmarks.Creator
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreatorDefaultParameterBenchmark
    {
        private const int Property1Value = 100;
        private const long Property2Value = 50;

        public CreatorDefaultParameterBenchmark()
        {
            this.Table = new Table("Property1", "Property2");
            this.Table.AddRow(Property1Value.ToString(), Property2Value.ToString());
            this.TableOneMatch = new Table("Property1");
            this.TableOneMatch.AddRow(Property1Value.ToString());
        }

        public Table Table { get; set; }
        public Table TableOneMatch { get; set; }

        [Benchmark(Baseline = true)]
        public StructWithDefaultParameter Ctor_Struct()
        {
            return this.Table.ToInstance<StructWithDefaultParameter>();
        }

        [Benchmark]
        public StructWithDefaultParameter Ctor_Struct_DefaultParameter()
        {
            return this.TableOneMatch.ToInstance<StructWithDefaultParameter>();
        }
    }
}