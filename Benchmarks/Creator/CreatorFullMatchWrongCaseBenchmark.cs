using BenchmarkDotNet.Attributes;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;

namespace InstanceCreator.Benchmarks.Creator
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreatorFullMatchWrongCaseBenchmark
    {
        private const int Property1Value = 100;
        private const long Property2Value = 50;

        public CreatorFullMatchWrongCaseBenchmark()
        {
            this.Table = new Table("Property1", "Property2");
            this.Table.AddRow(Property1Value.ToString(), Property2Value.ToString());
            this.TableWrongCase = new Table("prOPerTy1", "PrOpeRtY2");
            this.TableWrongCase.AddRow(Property1Value.ToString(), Property2Value.ToString());
        }

        public Table Table { get; set; }
        public Table TableWrongCase { get; set; }

        [Benchmark(Baseline = true)]
        public StructWithTwoProperties Prop_Struct()
        {
            return this.Table.ToInstance<StructWithTwoProperties>();
        }

        [Benchmark]
        public StructWithTwoProperties Prop_Struct_WrongCase()
        {
            return this.TableWrongCase.ToInstance<StructWithTwoProperties>();
        }
    }
}