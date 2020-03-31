using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist.InstanceBuilder;
using Xunit;

namespace InstanceCreator.Tests
{
    public class Test
    {
        [Fact]
        public void Can_create_an_instance_with_similar_enum_values()
        {
            var expectation = new AClassWithMultipleEnums
            {
                FirstColor = AClassWithMultipleEnums.Color.Red,
                SecondColor = AClassWithMultipleEnums.ColorAgain.Red,
                ThirdColor = AClassWithMultipleEnums.Color.Red,
                FourthColor = AClassWithMultipleEnums.ColorAgain.Green
            };
            var table = new Table(nameof(expectation.FirstColor), nameof(expectation.SecondColor), nameof(expectation.ThirdColor), nameof(expectation.FourthColor));
            table.AddRow(expectation.FirstColor.ToString(), expectation.SecondColor.ToString(), expectation.ThirdColor.ToString(), expectation.FourthColor.ToString());

            var result = TableInstanceCreatorExtensions.Test<AClassWithMultipleEnums>(table);

            result.Should().BeEquivalentTo(expectation);
        }

        [Fact]
        public void Can_create_an_instance_with_a_constructor_with_default_parameters()
        {
            var expectation = new AClassWithAConstructorWithDefaultParameters("Entry1", null);
            var table = new Table(nameof(expectation.Field1));
            table.AddRow(expectation.Field1);

            var result = TableInstanceCreatorExtensions.Test<AClassWithAConstructorWithDefaultParameters>(table);

            result.Should().BeEquivalentTo(expectation);
        }

        [Fact]
        public void Can_create_an_instance_with_a_constructor()
        {
            var expectation = new AClassWithAConstructor("Value1", "Value2");
            var table = new Table(nameof(expectation.Field1), nameof(expectation.Field2));
            table.AddRow(expectation.Field1, expectation.Field2);
            
            var result = TableInstanceCreatorExtensions.Test<AClassWithAConstructor>(table);

            result.Should().BeEquivalentTo(expectation);
        }

        [Fact]
        public void Can_create_an_instance_with_default_constructor_and_properties()
        {
            var expectation = new AClassWithDefaultConstructorAndProperties { StringProperty1 = "Value1", StringProperty2 = "Value2" };
            var table = new Table(nameof(expectation.StringProperty1), nameof(expectation.StringProperty2));
            table.AddRow(expectation.StringProperty1, expectation.StringProperty2);
            
            var result = TableInstanceCreatorExtensions.Test<AClassWithDefaultConstructorAndProperties>(table);

            result.Should().BeEquivalentTo(expectation);
        }


        [Fact]
        public void Can_create_an_instance_with_default_constructor_and_properties_with_missing_value()
        {
            var expectation = new AClassWithDefaultConstructorAndProperties { StringProperty1 = "Value1" };
            var table = new Table(nameof(expectation.StringProperty1));
            table.AddRow(expectation.StringProperty1);
            
            var result = TableInstanceCreatorExtensions.Test<AClassWithDefaultConstructorAndProperties>(table);

            result.Should().BeEquivalentTo(expectation);
        }

        private class AClassWithDefaultConstructorAndProperties
        {
            public string StringProperty1 { get; set; }
            public string StringProperty2 { get; set; }
        }

        public class AClassWithMultipleEnums
        {
            public Color FirstColor { get; set; }
            public ColorAgain SecondColor { get; set; }
            public Color ThirdColor { get; set; }
            public ColorAgain FourthColor { get; set; }

            public enum Color { Red, Green, Blue }
            public enum ColorAgain { Red, Green, Blue }
        }

        public class AClassWithAConstructor
        {
            public AClassWithAConstructor(string field1, string field2)
            {
                Field1 = field1;
                Field2 = field2;
            }

            public string Field1 { get; }
            public string Field2 { get; }
            public string Field3 { get; set; }
        }

        public class AClassWithAConstructorWithDefaultParameters
        {
            public AClassWithAConstructorWithDefaultParameters(string field1, string field2, string field3 = "Value3")
            {
                Field1 = field1;
                Field2 = field2;
                Field3 = field3;
            }

            public string Field1 { get; }
            public string Field2 { get; }
            public string Field3 { get; }
        }
    }
}
