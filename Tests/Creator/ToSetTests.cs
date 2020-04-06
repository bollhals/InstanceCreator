using System;
using System.Collections.Generic;
using FluentAssertions;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;
using Xunit;

namespace InstanceCreator.Tests.Creator
{
    public class ToSetTests
    {
        private const int Property1Value = 100;
        private const long Property2Value = 50;

        private static Table GetTwoNumberTable()
        {
            var table = new Table("Property1", "Property2");
            table.AddRow(Property1Value.ToString(), Property2Value.ToString());
            table.AddRow((Property1Value + 1).ToString(), (Property2Value + 1).ToString());
            return table;
        }
        
        private static Table GetVerticalTable()
        {
            var table = new Table("Field", "Value");
            table.AddRow("Property1", Property1Value.ToString());
            table.AddRow("Property2", Property2Value.ToString());
            return table;
        }
        
        public static TheoryData<object> FullMatchTwoPropertiesTestData => new TheoryData<object>
        {
            new List<ClassWithTwoProperties> { new ClassWithTwoProperties { Property1 = Property1Value, Property2 = Property2Value }, new ClassWithTwoProperties { Property1 = Property1Value + 1, Property2 = Property2Value + 1 } },
            new List<ClassWithTwoPropertiesAndConstructor> { new ClassWithTwoPropertiesAndConstructor(Property1Value, Property2Value), new ClassWithTwoPropertiesAndConstructor(Property1Value + 1, Property2Value + 1) },
            new List<StructWithTwoProperties> { new StructWithTwoProperties { Property1 = Property1Value, Property2 = Property2Value }, new StructWithTwoProperties { Property1 = Property1Value + 1, Property2 = Property2Value + 1 } },
            new List<StructWithTwoPropertiesAndConstructor> { new StructWithTwoPropertiesAndConstructor(Property1Value, Property2Value), new StructWithTwoPropertiesAndConstructor(Property1Value + 1, Property2Value + 1) },
        };

        [Theory]
        [MemberData(nameof(FullMatchTwoPropertiesTestData))]
        public void FullMatchTwoProperties<T>(List<T> expectation)
        {
            var instance = GetTwoNumberTable().ToSet<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
        
        [Fact]
        public void VerticalTable_ThrowsArgumentException()
        {
            Action action = () => GetVerticalTable().ToSet<ClassWithTwoProperties>();

            action.Should().Throw<ArgumentException>();
        }
    }
}