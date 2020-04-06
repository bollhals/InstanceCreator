using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using FluentAssertions;
using InstanceCreator.Converter;
using InstanceCreator.Tests.Types;
using Xunit;

namespace InstanceCreator.Tests.Converter
{
    public class DefaultConverterTests
    {
        private const bool DefaultIsNullValueValid = false;
        private const NumberStyles DefaultIntegerNumberStyles = NumberStyles.Integer;
        private const NumberStyles DefaultFloatingPointNumberStyles = NumberStyles.Float;
        private const DateTimeStyles DefaultDateTimeStyles = DateTimeStyles.None;

        [Theory]
        [InlineData("random", "random", true)]
        [InlineData(null, null, false)]
        [InlineData("", "", true)]
        public void Convert_String(string value, string expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<string>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        [Theory]
        [InlineData(null, null, true)]
        public void Convert_String_NullValue(string value, string expectation, bool expectConversion)
        {
            DefaultConverter.IsStringNullValid = true;
            var isConverted = TypeConverter<string>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IsStringNullValid = DefaultIsNullValueValid;

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        public static TheoryData<string, bool, bool> BoolTestData => new TheoryData<string, bool, bool>
        {
            { "True", true, true },
            { "true", true, true },
            { "tRUe", true, true },
            { "True ", true, true },
            { " True", true, true },
            { "Tr ue", default, false },
            { "1", default, false },
            { "False", false, true },
            { "false", false, true },
            { "fALSe", false, true },
            { "False ", false, true },
            { " false", false, true },
            { "Fa lse", default, false },
            { "0", default, false },
        };

        [Theory]
        [MemberData(nameof(BoolTestData))]
        [InlineData(null, default(bool), false)]
        [InlineData("", default(bool), false)]
        public void Convert_Bool(string value, bool expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<bool>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        [Theory]
        [MemberData(nameof(BoolTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Bool_Nullable(string value, bool? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<bool?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        public static TheoryData<string, char, bool> CharTestData => new TheoryData<string, char, bool>
        {
            { "0", '0', true },
            { "f", 'f', true },
            { "A", 'A', true },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(CharTestData))]
        [InlineData(null, default(char), false)]
        [InlineData("", default(char), false)]
        public void Convert_Char(string value, char expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<char>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Theory]
        [MemberData(nameof(CharTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Char_Nullable(string value, char? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<char?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        public static TheoryData<string, byte, bool> ByteTestData => new TheoryData<string, byte, bool>
        {
            { "0", 0, true },
            { "FF", 0, false },
            { "100", 100, true },
            { "255", byte.MaxValue, true },
            { "256", default, false },
            { "-1", default, false },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(ByteTestData))]
        [InlineData(null, default(byte), false)]
        [InlineData("", default(byte), false)]
        public void Convert_Byte(string value, byte expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<byte>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Byte_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<byte>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        [Theory]
        [MemberData(nameof(ByteTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Byte_Nullable(string value, byte? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<byte?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Byte_Nullable_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<byte?>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        public static TheoryData<string, sbyte, bool> SByteTestData => new TheoryData<string, sbyte, bool>
        {
            { "0", 0, true },
            { "0F", 0, false },
            { "100", 100, true },
            { "127", sbyte.MaxValue, true },
            { "128", default, false },
            { "-1", -1, true },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(SByteTestData))]
        [InlineData(null, default(sbyte), false)]
        [InlineData("", default(sbyte), false)]
        public void Convert_SByte(string value, sbyte expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<sbyte>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_SByte_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<sbyte>.Convert("0F", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0x0F);
        }

        [Theory]
        [MemberData(nameof(SByteTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_SByte_Nullable(string value, sbyte? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<sbyte?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_SByte_Nullable_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<sbyte?>.Convert("0F", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0x0F);
        }

        public static TheoryData<string, short, bool> ShortTestData => new TheoryData<string, short, bool>
        {
            { "0", 0, true },
            { "FF", 0, false },
            { "100", 100, true },
            { "-500", -500, true },
            { "32767", short.MaxValue, true },
            { "32768", default, false },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(ShortTestData))]
        [InlineData(null, default(short), false)]
        [InlineData("", default(short), false)]
        public void Convert_Short(string value, short expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<short>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Short_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<short>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        [Theory]
        [MemberData(nameof(ShortTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Short_Nullable(string value, short? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<short?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Short_Nullable_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<short?>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        public static TheoryData<string, ushort, bool> UShortTestData => new TheoryData<string, ushort, bool>
        {
            { "0", 0, true },
            { "FF", 0, false },
            { "100", 100, true },
            { "-500", 0, false },
            { "65535", ushort.MaxValue, true },
            { "65536", default, false },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(UShortTestData))]
        [InlineData(null, default(ushort), false)]
        [InlineData("", default(ushort), false)]
        public void Convert_UShort(string value, ushort expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<ushort>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_UShort_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<ushort>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        [Theory]
        [MemberData(nameof(UShortTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_UShort_Nullable(string value, ushort? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<ushort?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_UShort_Nullable_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<ushort?>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        public static TheoryData<string, int, bool> IntTestData => new TheoryData<string, int, bool>
        {
            { "0", 0, true },
            { "FF", 0, false },
            { "100", 100, true },
            { "-500", -500, true },
            { "2147483647", int.MaxValue, true },
            { "2147483648", default, false },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(IntTestData))]
        [InlineData(null, default(int), false)]
        [InlineData("", default(int), false)]
        public void Convert_Int(string value, int expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<int>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Int_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<int>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        [Theory]
        [MemberData(nameof(IntTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Int_Nullable(string value, int? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<int?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Int_Nullable_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<int?>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        public static TheoryData<string, uint, bool> UIntTestData => new TheoryData<string, uint, bool>
        {
            { "0", 0, true },
            { "FF", 0, false },
            { "100", 100, true },
            { "-500", 0, false },
            { "4294967295", uint.MaxValue, true },
            { "4294967296", default, false },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(UIntTestData))]
        [InlineData(null, default(uint), false)]
        [InlineData("", default(uint), false)]
        public void Convert_UInt(string value, uint expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<uint>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_UInt_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<uint>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        [Theory]
        [MemberData(nameof(UIntTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_UInt_Nullable(string value, uint? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<uint?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_UInt_Nullable_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<uint?>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        public static TheoryData<string, long, bool> LongTestData => new TheoryData<string, long, bool>
        {
            { "0", 0, true },
            { "FF", 0, false },
            { "100", 100, true },
            { "-500", -500, true },
            { "9223372036854775807", long.MaxValue, true },
            { "9223372036854775808", default, false },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(LongTestData))]
        [InlineData(null, default(long), false)]
        [InlineData("", default(long), false)]
        public void Convert_Long(string value, long expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<long>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Long_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<long>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        [Theory]
        [MemberData(nameof(LongTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Long_Nullable(string value, long? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<long?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        [Fact]
        public void Convert_Long_Nullable_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<long?>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        public static TheoryData<string, ulong, bool> ULongTestData => new TheoryData<string, ulong, bool>
        {
            { "0", 0, true },
            { "FF", 0, false },
            { "100", 100, true },
            { "-500", 0, false },
            { "18446744073709551615", ulong.MaxValue, true },
            { "18446744073709551616", default, false },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(ULongTestData))]
        [InlineData(null, default(ulong), false)]
        [InlineData("", default(ulong), false)]
        public void Convert_ULong(string value, ulong expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<ulong>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_ULong_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<ulong>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        [Theory]
        [MemberData(nameof(ULongTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_ULong_Nullable(string value, ulong? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<ulong?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        [Fact]
        public void Convert_ULong_Nullable_NumberStyle()
        {
            DefaultConverter.IntegerNumberStyles = NumberStyles.HexNumber;
            var isConverted = TypeConverter<ulong?>.Convert("FF", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.IntegerNumberStyles = DefaultIntegerNumberStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(0xFF);
        }

        public static TheoryData<string, float, bool> FloatTestData => new TheoryData<string, float, bool>
        {
            { "0", 0f, true },
            { "100", 100f, true },
            { "-500", -500f, true },
            { "2.556", 2.556f, true },
            { "3.402823466e+38", float.MaxValue, true },
            { "3.402824466e+38", float.PositiveInfinity, true },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(FloatTestData))]
        [InlineData(null, default(float), false)]
        [InlineData("", default(float), false)]
        public void Convert_Float(string value, float expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<float>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Float_NumberStyle()
        {
            DefaultConverter.FloatingPointNumberStyles = NumberStyles.Float & ~NumberStyles.AllowExponent;
            var isConverted = TypeConverter<float>.Convert("5.5e8", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.FloatingPointNumberStyles = DefaultFloatingPointNumberStyles;

            isConverted.Should().Be(false);
            actual.Should().Be(default);
        }

        [Theory]
        [MemberData(nameof(FloatTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Float_Nullable(string value, float? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<float?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Float_Nullable_NumberStyle()
        {
            DefaultConverter.FloatingPointNumberStyles = NumberStyles.Float & ~NumberStyles.AllowExponent;
            var isConverted = TypeConverter<float?>.Convert("5.5e8", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.FloatingPointNumberStyles = DefaultFloatingPointNumberStyles;

            isConverted.Should().Be(false);
            actual.Should().Be(default);
        }

        public static TheoryData<string, double, bool> DoubleTestData => new TheoryData<string, double, bool>
        {
            { "0", 0d, true },
            { "100", 100d, true },
            { "-500", -500d, true },
            { "2.556", 2.556d, true },
            { "1.7976931348623157E+308", double.MaxValue, true },
            { "1.7976931349000000E+308", double.PositiveInfinity, true },
            { "4string", default, false },
        };

        [Theory]
        [MemberData(nameof(DoubleTestData))]
        [InlineData(null, default(double), false)]
        [InlineData("", default(double), false)]
        public void Convert_Double(string value, double expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<double>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Double_NumberStyle()
        {
            DefaultConverter.FloatingPointNumberStyles = NumberStyles.Float & ~NumberStyles.AllowExponent;
            var isConverted = TypeConverter<double>.Convert("5.5e8", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.FloatingPointNumberStyles = DefaultFloatingPointNumberStyles;

            isConverted.Should().Be(false);
            actual.Should().Be(default);
        }

        [Theory]
        [MemberData(nameof(DoubleTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Double_Nullable(string value, double? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<double?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Double_Nullable_NumberStyle()
        {
            DefaultConverter.FloatingPointNumberStyles = NumberStyles.Float & ~NumberStyles.AllowExponent;
            var isConverted = TypeConverter<double?>.Convert("5.5e8", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.FloatingPointNumberStyles = DefaultFloatingPointNumberStyles;

            isConverted.Should().Be(false);
            actual.Should().Be(default);
        }

        public static TheoryData<string, decimal, bool> DecimalTestData => new TheoryData<string, decimal, bool>
        {
            { "0", 0m, true },
            { "100", 100m, true },
            { "-500", -500m, true },
            { "2.556", 2.556m, true },
            { "79228162514264337593543950335", decimal.MaxValue, true },
            { "79228162514264337593543950336", default, false },
            { "4string", default, false },
        };
        
        public static TheoryData<string, decimal, bool> DecimalTestDataExtra => new TheoryData<string, decimal, bool>
        {
            { null, default, false },
            { "", default, false },
        };

        [Theory]
        [MemberData(nameof(DecimalTestData))]
        [MemberData(nameof(DecimalTestDataExtra))]
        public void Convert_Decimal(string value, decimal expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<decimal>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Decimal_NumberStyle()
        {
            DefaultConverter.FloatingPointNumberStyles = NumberStyles.Float & ~NumberStyles.AllowExponent;
            var isConverted = TypeConverter<decimal>.Convert("5.5e8", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.FloatingPointNumberStyles = DefaultFloatingPointNumberStyles;

            isConverted.Should().Be(false);
            actual.Should().Be(default);
        }

        [Theory]
        [MemberData(nameof(DecimalTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Decimal_Nullable(string value, decimal? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<decimal?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_Decimal_Nullable_NumberStyle()
        {
            DefaultConverter.FloatingPointNumberStyles = NumberStyles.Float & ~NumberStyles.AllowExponent;
            var isConverted = TypeConverter<decimal?>.Convert("5.5e8", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.FloatingPointNumberStyles = DefaultFloatingPointNumberStyles;

            isConverted.Should().Be(false);
            actual.Should().Be(default);
        }

        public static TheoryData<string, DateTime, bool> DateTimeTestData => new TheoryData<string, DateTime, bool>
        {
            { "2020-04-10", new DateTime(2020, 04, 10), true },
            { "2020-04-10T12:13:14", new DateTime(2020, 4, 10, 12, 13, 14), true },
            { "2020-04-10T12:13:14.012", new DateTime(2020, 4, 10, 12, 13, 14, 012), true },
            { "20200-04-10T12:13:14.012", DateTime.MinValue, false },
            { "2020-RANDOM", DateTime.MinValue, false },
        };
        
        public static TheoryData<string, DateTime, bool> DateTimeTestDataExtra => new TheoryData<string, DateTime, bool>
        {
            { null, default, false },
            { "", default, false },
        };

        [Theory]
        [MemberData(nameof(DateTimeTestData))]
        [MemberData(nameof(DateTimeTestDataExtra))]
        public void Convert_DateTime(string value, DateTime expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<DateTime>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_DateTime_DateTimeStyle()
        {
            DefaultConverter.DateTimeStyles = DateTimeStyles.AdjustToUniversal;
            var isConverted = TypeConverter<DateTime>.Convert("2020-04-10T12:13:14", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.DateTimeStyles = DefaultDateTimeStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(new DateTime(2020, 4, 10, 12, 13, 14, DateTimeKind.Utc));
        }

        [Theory]
        [MemberData(nameof(DateTimeTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_DateTime_Nullable(string value, DateTime? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<DateTime?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_DateTime_Nullable_DateTimeStyle()
        {
            DefaultConverter.DateTimeStyles = DateTimeStyles.AdjustToUniversal;
            var isConverted = TypeConverter<DateTime?>.Convert("2020-04-10T12:13:14", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.DateTimeStyles = DefaultDateTimeStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(new DateTime(2020, 4, 10, 12, 13, 14, DateTimeKind.Utc));
        }

        public static TheoryData<string, DateTimeOffset, bool> DateTimeOffsetTestData => new TheoryData<string, DateTimeOffset, bool>
        {
            { "2020-04-10+00:00", new DateTimeOffset(2020, 04, 10, 0, 0, 0, TimeSpan.Zero), true },
            { "2020-04-10T12:13:14+00:00", new DateTimeOffset(2020, 4, 10, 12, 13, 14, TimeSpan.Zero), true },
            { "2020-04-10T12:13:14.012+00:00", new DateTimeOffset(2020, 4, 10, 12, 13, 14, 012, TimeSpan.Zero), true },
            { "2020-04-10T12:13:14.012+08:30", new DateTimeOffset(2020, 4, 10, 12, 13, 14, 012, TimeSpan.FromHours(8.5)), true },
            { "20200-04-10T12:13:14.012", DateTimeOffset.MinValue, false },
            { "2020-RANDOM", DateTimeOffset.MinValue, false },
        };
        
        public static TheoryData<string, DateTimeOffset, bool> DateTimeOffsetTestDataExtra => new TheoryData<string, DateTimeOffset, bool>
        {
            { null, default, false },
            { "", default, false },
        };

        [Theory]
        [MemberData(nameof(DateTimeOffsetTestData))]
        [MemberData(nameof(DateTimeOffsetTestDataExtra))]
        public void Convert_DateTimeOffset(string value, DateTimeOffset expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<DateTimeOffset>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_DateTimeOffset_DateTimeStyle()
        {
            DefaultConverter.DateTimeStyles = DateTimeStyles.AdjustToUniversal;
            var isConverted = TypeConverter<DateTimeOffset>.Convert("2020-04-10T12:13:14+02:00", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.DateTimeStyles = DefaultDateTimeStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(new DateTimeOffset(2020, 4, 10, 12, 13, 14, TimeSpan.FromHours(2)));
            actual.Offset.Should().Be(TimeSpan.Zero);
        }

        [Theory]
        [MemberData(nameof(DateTimeOffsetTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_DateTimeOffset_Nullable(string value, DateTimeOffset? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<DateTimeOffset?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        [Fact]
        public void Convert_DateTimeOffset_Nullable_DateTimeStyle()
        {
            DefaultConverter.DateTimeStyles = DateTimeStyles.AdjustToUniversal;
            var isConverted = TypeConverter<DateTimeOffset?>.Convert("2020-04-10T12:13:14+02:00", CultureInfo.InvariantCulture, out var actual);
            DefaultConverter.DateTimeStyles = DefaultDateTimeStyles;

            isConverted.Should().Be(true);
            actual.Should().Be(new DateTimeOffset(2020, 4, 10, 12, 13, 14, TimeSpan.FromHours(2)));
            actual.Value.Offset.Should().Be(TimeSpan.Zero);
        }

        public static TheoryData<string, ColorEnum, bool> EnumTestData => new TheoryData<string, ColorEnum, bool>
        {
            { "Red", ColorEnum.Red, true },
            { "Blue", ColorEnum.Blue, true },
            { "nonEnum", ColorEnum.Undefined, false },
        };
        
        [Theory]
        [MemberData(nameof(EnumTestData))]
        [InlineData(null, ColorEnum.Undefined, false)]
        [InlineData("", ColorEnum.Undefined, false)]
        public void Convert_Enum(string value, ColorEnum expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<ColorEnum>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        [Theory]
        [MemberData(nameof(EnumTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Enum_Nullable(string value, ColorEnum? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<ColorEnum?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        public static TheoryData<string, OptionFlags, bool> FlagsTestData => new TheoryData<string, OptionFlags, bool>
        {
            { "A", OptionFlags.A, true },
            { "B", OptionFlags.B, true },
            { "nonEnum", OptionFlags.None, false },
            { "A, B", OptionFlags.A | OptionFlags.B, true },
        };

        [Theory]
        [MemberData(nameof(FlagsTestData))]
        [InlineData(null, OptionFlags.None, false)]
        [InlineData("", OptionFlags.None, false)]
        public void Convert_Flags(string value, OptionFlags expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<OptionFlags>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        [Theory]
        [MemberData(nameof(FlagsTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Flags_Nullable(string value, OptionFlags? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<OptionFlags?>.Convert(value, CultureInfo.InvariantCulture, out var actual);

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        public static TheoryData<string, Guid, bool> GuidTestData => new TheoryData<string, Guid, bool>
        {
            { "D4DF7201-3663-49FF-8B4F-908B3CE444C8", Guid.Parse("D4DF7201-3663-49FF-8B4F-908B3CE444C8"), true },
            { "{C00726C7-D6FF-43D3-BC5E-F3D30A502333}", Guid.Parse("{C00726C7-D6FF-43D3-BC5E-F3D30A502333}"), true },
            { "(BE45B7D7-657A-4AD6-890E-1DB4B4E680E3)", Guid.Parse("(BE45B7D7-657A-4AD6-890E-1DB4B4E680E3)"), true },
            { "65D2E145EA6A4FA5B7EE36B8FC0CD5B8", Guid.Parse("65D2E145EA6A4FA5B7EE36B8FC0CD5B8"), true },
            { "79cd5e56-fee8-4b9b-8b8a-96b5ab197e2f", Guid.Parse("79cd5e56-fee8-4b9b-8b8a-96b5ab197e2f"), true },
            { "{ceebd71f-1f10-4457-acb5-0610ca1dd97b}", Guid.Parse("{ceebd71f-1f10-4457-acb5-0610ca1dd97b}"), true },
            { "(2c895fc7-9f73-416f-a71b-99b720fb1367)", Guid.Parse("(2c895fc7-9f73-416f-a71b-99b720fb1367)"), true },
            { "1219b09a3cfa465aa78cd7e3de05bc92", Guid.Parse("1219b09a3cfa465aa78cd7e3de05bc92"), true },
            { "1219b09a3cfa465aa78cd7e3de05bc9233333", Guid.Empty, false },
            { "1219b09a3cfa465aa78cd7e3de05bc", Guid.Empty, false },
            { "79cd5e56-fee8-4b9b-8b8a-96b5ab1-7e2f", Guid.Empty, false },
            { "79cd5e56-fee8-4b9b-8b8a-96b5ab1-97e2f", Guid.Empty, false },
        };

        public static TheoryData<string, Guid, bool> GuidTestDataExtra => new TheoryData<string, Guid, bool>
        {
            { null, Guid.Empty, false },
            { "", Guid.Empty, false },
        };

        [Theory]
        [MemberData(nameof(GuidTestData))]
        [MemberData(nameof(GuidTestDataExtra))]
        public void Convert_Guid(string value, Guid expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<Guid>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        [Theory]
        [MemberData(nameof(GuidTestData))]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        public void Convert_Guid_Nullable(string value, Guid? expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<Guid?>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }
        
        public static TheoryData<string, int[], bool> ArrayTestData => new TheoryData<string, int[], bool>
        {
            { "1, 2, 3", new []{ 1, 2, 3 }, true },
            { "1, nonValue, 3", new []{ 1, 0, 3 }, false },
            { "nonValue", new []{ 0 }, false },
            { null, null, true },
            { "", Array.Empty<int>(), true },
        };

        [Theory]
        [MemberData(nameof(ArrayTestData))]
        public void Convert_Array(string value, int[] expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<int[]>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().BeEquivalentTo(expectation);
        }

        [Theory]
        [MemberData(nameof(ArrayTestData))]
        public void Convert_IEnumerable(string value, IEnumerable<int> expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<IEnumerable<int>>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().BeEquivalentTo(expectation);
        }

        [Theory]
        [MemberData(nameof(ArrayTestData))]
        public void Convert_IReadOnlyList(string value, IReadOnlyList<int> expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<IReadOnlyList<int>>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().BeEquivalentTo(expectation);
        }

        [Theory]
        [MemberData(nameof(ArrayTestData))]
        public void Convert_ICollection(string value, ICollection<int> expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<ICollection<int>>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().BeEquivalentTo(expectation);
        }

        [Theory]
        [MemberData(nameof(ArrayTestData))]
        public void Convert_IReadOnlyCollection(string value, IReadOnlyCollection<int> expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<IReadOnlyCollection<int>>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().BeEquivalentTo(expectation);
        }
        
        public static TheoryData<string, Collection<int>, bool> CollectionTestData => new TheoryData<string, Collection<int>, bool>
        {
            { "1, 2, 3", new Collection<int> { 1, 2, 3 }, true },
            { "1, nonValue, 3", new Collection<int> { 1, 0, 3 }, false },
            { "nonValue", new Collection<int> { 0 }, false },
            { null, null, true },
            { "", new Collection<int>(), true },
        };

        [Theory]
        [MemberData(nameof(CollectionTestData))]
        public void Convert_Collection(string value, Collection<int> expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<Collection<int>>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().BeEquivalentTo(expectation);
        }

        public static TheoryData<string, List<int>, bool> ListTestData => new TheoryData<string, List<int>, bool>
        {
            { "1, 2, 3", new List<int> { 1, 2, 3 }, true },
            { "1, nonValue, 3", new List<int> { 1, 0, 3 }, false },
            { "nonValue", new List<int> { 0 }, false },
            { null, null, true },
            { "", new List<int>(), true },
        };

        [Theory]
        [MemberData(nameof(ListTestData))]
        public void Convert_List(string value, List<int> expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<List<int>>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().BeEquivalentTo(expectation);
        }

        [Theory]
        [InlineData("anything", null, false)]
        public void Convert_Default(string value, object expectation, bool expectConversion)
        {
            var isConverted = TypeConverter<object>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            
            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

        [Theory]
        [InlineData("500", 20, true)]
        public void Convert_Overwrite(string value, int expectation, bool expectConversion)
        {
            TypeConverter<int>.Convert = (string s, CultureInfo culture, out int i) =>
            {
                i = 20;
                return true;
            };
            var isConverted = TypeConverter<int>.Convert(value, CultureInfo.InvariantCulture, out var actual);
            TypeConverter<int>.Convert = null;

            isConverted.Should().Be(expectConversion);
            actual.Should().Be(expectation);
        }

    }
}