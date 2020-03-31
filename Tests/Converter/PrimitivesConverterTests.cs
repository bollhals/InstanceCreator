using System;
using System.Collections.Generic;
using FluentAssertions;
using InstanceCreator.Converter;
using InstanceCreator.Tests.Types;
using Xunit;

namespace InstanceCreator.Tests.Converter
{
    public class PrimitivesConverterTests
    {
        private readonly BasicTypesConverter testee;

        public PrimitivesConverterTests()
        {
            this.testee = new BasicTypesConverter();
        }

        public static IEnumerable<object[]> ConvertTestData()
        {
            yield return new object[] { "true", typeof(bool), true, true };
            yield return new object[] { "F", typeof(char), true, 'F' };
            yield return new object[] { "Too long", typeof(char), false, null };
            yield return new object[] { "10", typeof(byte), true, (byte)10 };
            yield return new object[] { "10", typeof(sbyte), true, (sbyte)10 };
            yield return new object[] { "10", typeof(short), true, (short)10 };
            yield return new object[] { "10", typeof(ushort), true, (ushort)10 };
            yield return new object[] { "10", typeof(int), true, (int)10 };
            yield return new object[] { "10", typeof(uint), true, (uint)10 };
            yield return new object[] { "10", typeof(long), true, (long)10 };
            yield return new object[] { "10", typeof(ulong), true, (ulong)10 };
            yield return new object[] { "10", typeof(decimal), true, (decimal)10 };
            yield return new object[] { "10", typeof(float), true, (float)10 };
            yield return new object[] { "10", typeof(double), true, (double)10 };
            yield return new object[] { "10", typeof(long?), true, (long?)10 };
            yield return new object[] { null, typeof(long?), true, (long?)null };
            yield return new object[] { "Red", typeof(ColorEnum), true, ColorEnum.Red };
            yield return new object[] { "Random", typeof(string), true, "Random" };
            yield return new object[] { null, typeof(string), true, null };
            yield return new object[] { "2020-12-11T10:09:08", typeof(DateTime), true, new DateTime(2020, 12, 11, 10, 9, 8, DateTimeKind.Local) };
            yield return new object[] { null, typeof(DateTime?), true, null };

            var guid = "51EE6A50-F64D-4A39-881D-42C74B1955E2";
            yield return new object[] { guid, typeof(Guid), true, Guid.Parse(guid) };
        }

        [Theory]
        [MemberData(nameof(ConvertTestData))]
        public void Convert(string value, Type type, bool expectConversion, object expectation)
        {
            var converted = this.testee.TryConvert(value, type, out var result);

            result.Should().BeEquivalentTo(expectation);
            converted.Should().Be(expectConversion);
        }
    }
}