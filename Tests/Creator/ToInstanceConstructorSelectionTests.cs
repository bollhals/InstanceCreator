using FluentAssertions;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;
using Xunit;

namespace InstanceCreator.Tests.Creator
{
    public class ToInstanceConstructorSelectionTests
    {
        private const int Property1Value = 300;
        private const long Property2Value = 5000;
        private const bool Property3Value = true;
        
        private static Table GetTwoNumberTable()
        {
            var table = new Table("Property1", "Property2");
            table.AddRow(Property1Value.ToString(), Property2Value.ToString());
            return table;
        }

        private static Table GetProperty3Table()
        {
            var table = new Table("Property3");
            table.AddRow(Property3Value.ToString());
            return table;
        }

        private static Table GetProperty2Table()
        {
            var table = new Table("Property2");
            table.AddRow(Property2Value.ToString());
            return table;
        }
        
        public static TheoryData<object> FullMatchTestData => new TheoryData<object>
        {
            new ClassWithMultipleConstructors(Property1Value, Property2Value),
            new StructWithMultipleConstructors(Property1Value, Property2Value),
        };
        
        [Theory]
        [MemberData(nameof(FullMatchTestData))]
        public void FullMatchTwoPropertiesConstructor<T>(T expectation)
        {
            var instance = GetTwoNumberTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
        
        public static TheoryData<object> PartialMatchTestData => new TheoryData<object>
        {
            new ClassWithMultipleConstructors(default, Property2Value),
            new StructWithMultipleConstructors(default, Property2Value),
        };

        [Theory]
        [MemberData(nameof(PartialMatchTestData))]
        public void PartialMatchTwoPropertiesConstructor<T>(T expectation)
        {
            var instance = GetProperty2Table().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
        
        public static TheoryData<object> DefaultConstructorTestData => new TheoryData<object>
        {
            new ClassWithMultipleConstructors { Property3 = true },
            new StructWithMultipleConstructors { Property3 = true },
        };

        [Theory]
        [MemberData(nameof(DefaultConstructorTestData))]
        public void MatchDefaultConstructor<T>(T expectation)
        {
            var instance = GetProperty3Table().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
    }
}