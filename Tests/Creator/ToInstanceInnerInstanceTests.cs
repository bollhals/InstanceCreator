using FluentAssertions;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;
using Xunit;

namespace InstanceCreator.Tests.Creator
{
    public class ToInstanceInnerInstanceTests
    {
        private const int Property1Value = 100;
        private const long Property2Value = 50;
        private const int InnerProperty1Value = 80;
        private const long InnerProperty2Value = 90;

        private static Table GetTwoNumberTable()
        {
            var table = new Table("Property1", "Property2", "Inner.Property1", "Inner.Property2");
            table.AddRow(Property1Value.ToString(), Property2Value.ToString(), InnerProperty1Value.ToString(), InnerProperty2Value.ToString());
            return table;
        }
        
        private static Table GetTwoNumberWrongCaseTable()
        {
            var table = new Table("PrOpErTy1", "properTY2", "Inner.ProPERTY1", "Inner.proPertY2");
            table.AddRow(Property1Value.ToString(), Property2Value.ToString(), InnerProperty1Value.ToString(), InnerProperty2Value.ToString());
            return table;
        }
        
        private static Table GetOnlyInnerTable()
        {
            var table = new Table("Inner.Property1", "Inner.Property2");
            table.AddRow(InnerProperty1Value.ToString(), InnerProperty2Value.ToString());
            return table;
        }

        public static TheoryData<object> FullMatchTwoPropertiesTestData => new TheoryData<object>
        {
            new ClassInClass { Property1 = Property1Value, Property2 = Property2Value, Inner = new ClassWithTwoProperties { Property1 = InnerProperty1Value, Property2 = InnerProperty2Value }},
            new ClassInClassWithConstructor(Property1Value, Property2Value, new ClassWithTwoPropertiesAndConstructor(InnerProperty1Value, InnerProperty2Value)),
            new StructInStruct { Property1 = Property1Value, Property2 = Property2Value, Inner = new StructWithTwoProperties { Property1 = InnerProperty1Value, Property2 = InnerProperty2Value }},
            new StructInStructWithConstructor(Property1Value, Property2Value, new StructWithTwoPropertiesAndConstructor(InnerProperty1Value, InnerProperty2Value)),
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
            new ClassInClass { Inner = new ClassWithTwoProperties { Property1 = InnerProperty1Value, Property2 = InnerProperty2Value }},
            new ClassInClassWithConstructor(default, default, new ClassWithTwoPropertiesAndConstructor(InnerProperty1Value, InnerProperty2Value)),
            new StructInStruct { Inner = new StructWithTwoProperties { Property1 = InnerProperty1Value, Property2 = InnerProperty2Value }},
            new StructInStructWithConstructor(default, default, new StructWithTwoPropertiesAndConstructor(InnerProperty1Value, InnerProperty2Value)),
        };

        [Theory]
        [MemberData(nameof(OnePropertyMatchTwoPropertiesTestData))]
        public void FullInnerMatch<T>(T expectation)
        {
            var instance = GetOnlyInnerTable().ToInstance<T>();

            instance.Should().BeEquivalentTo(expectation);
        }
    }
}