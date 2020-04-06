using FluentAssertions;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;
using Xunit;

namespace InstanceCreator.Tests.Creator
{
    public class ToInstanceSingleTypeTests
    {
        private const int Property1Value = 100;
        private const long Property2Value = 50;

        private static Table GetTwoNumberTable()
        {
            var table = new Table("Property1", "Property2");
            table.AddRow(Property1Value.ToString(), Property2Value.ToString());
            return table;
        }
        
        private static Table GetTwoNumberWrongCaseTable()
        {
            var table = new Table("PrOpErTy1", "properTY2");
            table.AddRow(Property1Value.ToString(), Property2Value.ToString());
            return table;
        }
        
        private static Table GetOneNumberTable()
        {
            var table = new Table("Property1");
            table.AddRow(Property1Value.ToString());
            return table;
        }
        
        private static Table GetNoMatchTable()
        {
            var table = new Table("RANDOM");
            table.AddRow(Property1Value.ToString());
            return table;
        }
        
        private static Table GetOneNumberOneWrongValueTable()
        {
            var table = new Table("Property1", "Property2");
            table.AddRow("WrongValue", Property2Value.ToString());
            return table;
        }
        
        public static TheoryData<object> FullMatchTwoPropertiesTestData => new TheoryData<object>
        {
            new ClassWithTwoProperties { Property1 = Property1Value, Property2 = Property2Value },
            new ClassWithTwoPropertiesAndConstructor(Property1Value, Property2Value),
            new StructWithTwoProperties { Property1 = Property1Value, Property2 = Property2Value },
            new StructWithTwoPropertiesAndConstructor(Property1Value, Property2Value),
        };

        [Theory]
        [MemberData(nameof(FullMatchTwoPropertiesTestData))]
        public void FullMatchTwoProperties<T>(T expectation)
        {
            var instance = GetTwoNumberWrongCaseTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
        
        [Theory]
        [MemberData(nameof(FullMatchTwoPropertiesTestData))]
        public void FullMatchCaseInsensitiveTwoProperties<T>(T expectation)
        {
            var instance = GetTwoNumberTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
        
        public static TheoryData<object> OnePropertyMatchTwoPropertiesTestData => new TheoryData<object>
        {
            new ClassWithTwoProperties { Property1 = Property1Value, Property2 = default },
            new ClassWithTwoPropertiesAndConstructor(Property1Value, default),
            new StructWithTwoProperties { Property1 = Property1Value, Property2 = default },
            new StructWithTwoPropertiesAndConstructor(Property1Value, default),
        };

        [Theory]
        [MemberData(nameof(OnePropertyMatchTwoPropertiesTestData))]
        public void PartialMatchTwoProperties_OnlySetsMatches<T>(T expectation)
        {
            var instance = GetOneNumberTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }

        public static TheoryData<object> NoMatchTwoPropertiesTestData => new TheoryData<object>
        {
            new ClassWithTwoProperties { Property1 = default, Property2 = default },
            new ClassWithTwoPropertiesAndConstructor(default, default),
            new StructWithTwoProperties { Property1 = default, Property2 = default },
            new StructWithTwoPropertiesAndConstructor(default, default),
        };

        [Theory]
        [MemberData(nameof(NoMatchTwoPropertiesTestData))]
        public void NoMatchTwoProperties_DefaultsValues<T>(T expectation)
        {
            var instance = GetNoMatchTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
        
        public static TheoryData<object> ReadAndWritablePropertiesTestData => new TheoryData<object>
        {
            new ClassWithReadAndWritableProperties { Property1 = Property1Value },
            new StructWithReadAndWritableProperties { Property2 = Property2Value },
        };

        [Theory]
        [MemberData(nameof(ReadAndWritablePropertiesTestData))]
        public void ReadAndWritableProperties_OnlySetsWritable<T>(T expectation)
        {
            var instance = GetTwoNumberTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }

        public static TheoryData<object> OneMatchOneWrongValueTwoPropertiesTestData => new TheoryData<object>
        {
            new ClassWithTwoProperties { Property1 = default, Property2 = Property2Value },
            new ClassWithTwoPropertiesAndConstructor(default, Property2Value),
            new StructWithTwoProperties { Property1 = default, Property2 = Property2Value },
            new StructWithTwoPropertiesAndConstructor(default, Property2Value),
        };

        [Theory]
        [MemberData(nameof(OneMatchOneWrongValueTwoPropertiesTestData))]
        public void OneMatchOneWrongValueTwoProperties_DefaultsWrongValue<T>(T expectation)
        {
            var instance = GetOneNumberOneWrongValueTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }

        public static TheoryData<object> DefaultParameterNotSetTestData => new TheoryData<object>
        {
            new ClassWithDefaultParameter(Property1Value),
            new StructWithDefaultParameter(Property1Value),
        };

        [Theory]
        [MemberData(nameof(DefaultParameterNotSetTestData))]
        public void DefaultParameter_TakesDefault<T>(T expectation)
        {
            var instance = GetOneNumberTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }

        public static TheoryData<object> DefaultParameterSetTestData => new TheoryData<object>
        {
            new ClassWithDefaultParameter(Property1Value, Property2Value),
            new StructWithDefaultParameter(Property1Value, Property2Value),
        };

        [Theory]
        [MemberData(nameof(DefaultParameterSetTestData))]
        public void DefaultParameter_OverwritesDefault<T>(T expectation)
        {
            var instance = GetTwoNumberTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }

        public static TheoryData<object> DefaultParameterSetTestData2 => new TheoryData<object>
        {
            new ClassWithOnlyDefaultParameter(),
        };

        [Theory]
        [MemberData(nameof(DefaultParameterSetTestData2))]
        public void DefaultParameter_OverwritesDefault2<T>(T expectation)
        {
            var instance = GetNoMatchTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
    }
}