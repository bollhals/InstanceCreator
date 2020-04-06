using FluentAssertions;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;
using Xunit;

namespace InstanceCreator.Tests.Creator
{
    public class ToInstanceVerticalTableTests
    {
        private const int Property1Value = 300;
        private const long Property2Value = 5000;

        private static Table GetTwoNumberTable()
        {
            var table = new Table("Field", "Value");
            table.AddRow("Property1", Property1Value.ToString());
            table.AddRow("Property2", Property2Value.ToString());
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
            var instance = GetTwoNumberTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
    }
}