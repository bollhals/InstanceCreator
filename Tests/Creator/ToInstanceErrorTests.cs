using System;
using FluentAssertions;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;
using TechTalk.SpecFlow;
using Xunit;

namespace InstanceCreator.Tests.Creator
{
    public class ToInstanceErrorTests
    {
        private const int Property1Value = 300;

        private static Table GetNoMatchTable()
        {
            var table = new Table("RANDOM");
            table.AddRow(Property1Value.ToString());
            return table;
        }

        private static Table GetMultipleRowTable()
        {
            var table = new Table("RANDOM");
            table.AddRow(Property1Value.ToString());
            table.AddRow(Property1Value.ToString());
            return table;
        }

        [Fact]
        public void NoPublicConstructors_ThrowsInvalidOperationException()
        {
            Action action = () => GetNoMatchTable().ToInstance<ClassWithNoPublicConstructors>();

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void MultipleRows_ThrowsArgumentException()
        {
            Action action = () => GetMultipleRowTable().ToInstance<ClassWithNoPublicConstructors>();

            action.Should().Throw<ArgumentException>();
        }
    }
}