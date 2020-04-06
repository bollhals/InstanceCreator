using BenchmarkDotNet.Attributes;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace InstanceCreator.Benchmarks.Creator
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreatorDifferentMatchesBenchmark
    {
        private const int Property1Value = 100;
        private const long Property2Value = 50;

        public CreatorDifferentMatchesBenchmark()
        {
            this.TableFull = new Table("Property1", "Property2");
            this.TableFull.AddRow(Property1Value.ToString(), Property2Value.ToString());
            this.TablePartial = new Table("Property1");
            this.TablePartial.AddRow(Property1Value.ToString());
            this.TableNo = new Table("Random");
            this.TableNo.AddRow("Value");
        }

        public Table TableFull { get; set; }
        public Table TablePartial { get; set; }
        public Table TableNo { get; set; }

        [Benchmark]
        public ClassWithTwoProperties Prop_Class_Full()
        {
            return this.TableFull.ToInstance<ClassWithTwoProperties>();
        }

        [Benchmark]
        public ClassWithTwoProperties Prop_Class_Full_Original()
        {
            return this.TableFull.CreateInstance<ClassWithTwoProperties>();
        }

        [Benchmark]
        public ClassWithTwoProperties Prop_Class_Partial()
        {
            return this.TablePartial.ToInstance<ClassWithTwoProperties>();
        }

        [Benchmark]
        public ClassWithTwoProperties Prop_Class_Partial_Original()
        {
            return this.TablePartial.CreateInstance<ClassWithTwoProperties>();
        }

        [Benchmark]
        public ClassWithTwoProperties Prop_Class_No()
        {
            return this.TableNo.ToInstance<ClassWithTwoProperties>();
        }

        [Benchmark]
        public ClassWithTwoProperties Prop_Class_No_Original()
        {
            return this.TableNo.CreateInstance<ClassWithTwoProperties>();
        }
    }
}