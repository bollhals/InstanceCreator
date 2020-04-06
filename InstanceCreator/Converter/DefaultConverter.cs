using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace InstanceCreator.Converter
{
    public static class DefaultConverter
    {
        private static MethodInfo convertToEnumMethodInfo;
        private static MethodInfo convertToNullableEnumMethodInfo;
        private static MethodInfo convertToArrayMethodInfo;
        private static MethodInfo convertToIEnumerableMethodInfo;
        private static MethodInfo convertToListMethodInfo;
        private static MethodInfo convertToIReadOnlyListMethodInfo;
        private static MethodInfo convertToCollectionMethodInfo;
        private static MethodInfo convertToICollectionMethodInfo;
        private static MethodInfo convertToIReadOnlyCollectionMethodInfo;

        public static bool IsStringNullValid { get; set; } = false;
        public static NumberStyles IntegerNumberStyles { get; set; } = NumberStyles.Integer;
        public static NumberStyles FloatingPointNumberStyles { get; set; } = NumberStyles.Float;
        public static DateTimeStyles DateTimeStyles { get; set; } = DateTimeStyles.None;

        internal static Delegate Get<T>()
        {
            if (typeof(T) == typeof(string))
                return (ConverterFunc<string>) ToString;
            if (typeof(T) == typeof(char))
                return (ConverterFunc<char>) ToChar;
            if (typeof(T) == typeof(char?))
                return (ConverterFunc<char?>) ToCharNullable;
            if (typeof(T) == typeof(bool))
                return (ConverterFunc<bool>) ToBool;
            if (typeof(T) == typeof(bool?))
                return (ConverterFunc<bool?>) ToBoolNullable;
            if (typeof(T) == typeof(byte))
                return (ConverterFunc<byte>) ToByte;
            if (typeof(T) == typeof(byte?))
                return (ConverterFunc<byte?>) ToByteNullable;
            if (typeof(T) == typeof(sbyte))
                return (ConverterFunc<sbyte>) ToSByte;
            if (typeof(T) == typeof(sbyte?))
                return (ConverterFunc<sbyte?>) ToSByteNullable;
            if (typeof(T) == typeof(short))
                return (ConverterFunc<short>) ToShort;
            if (typeof(T) == typeof(short?))
                return (ConverterFunc<short?>) ToShortNullable;
            if (typeof(T) == typeof(ushort))
                return (ConverterFunc<ushort>) ToUShort;
            if (typeof(T) == typeof(ushort?))
                return (ConverterFunc<ushort?>) ToUShortNullable;
            if (typeof(T) == typeof(int))
                return (ConverterFunc<int>) ToInt;
            if (typeof(T) == typeof(int?))
                return (ConverterFunc<int?>) ToIntNullable;
            if (typeof(T) == typeof(uint))
                return (ConverterFunc<uint>) ToUInt;
            if (typeof(T) == typeof(uint?))
                return (ConverterFunc<uint?>) ToUIntNullable;
            if (typeof(T) == typeof(long))
                return (ConverterFunc<long>) ToLong;
            if (typeof(T) == typeof(long?))
                return (ConverterFunc<long?>) ToLongNullable;
            if (typeof(T) == typeof(ulong))
                return (ConverterFunc<ulong>) ToULong;
            if (typeof(T) == typeof(ulong?))
                return (ConverterFunc<ulong?>) ToULongNullable;
            if (typeof(T) == typeof(float))
                return (ConverterFunc<float>) ToFloat;
            if (typeof(T) == typeof(float?))
                return (ConverterFunc<float?>) ToFloatNullable;
            if (typeof(T) == typeof(double))
                return (ConverterFunc<double>) ToDouble;
            if (typeof(T) == typeof(double?))
                return (ConverterFunc<double?>) ToDoubleNullable;
            if (typeof(T) == typeof(decimal))
                return (ConverterFunc<decimal>) ToDecimal;
            if (typeof(T) == typeof(decimal?))
                return (ConverterFunc<decimal?>) ToDecimalNullable;
            if (typeof(T) == typeof(DateTime))
                return (ConverterFunc<DateTime>) ToDateTime;
            if (typeof(T) == typeof(DateTime?))
                return (ConverterFunc<DateTime?>) ToDateTimeNullable;
            if (typeof(T) == typeof(DateTimeOffset))
                return (ConverterFunc<DateTimeOffset>) ToDateTimeOffset;
            if (typeof(T) == typeof(DateTimeOffset?))
                return (ConverterFunc<DateTimeOffset?>) ToDateTimeOffsetNullable;
            if (typeof(T) == typeof(Guid))
                return (ConverterFunc<Guid>) ToGuidOffset;
            if (typeof(T) == typeof(Guid?))
                return (ConverterFunc<Guid?>) ToGuidOffsetNullable;

            if (typeof(T).IsEnum)
            {
                // We have to deal invoke this dynamically to work around the type constraint on Enum.TryParse.
                var method = convertToEnumMethodInfo ??= typeof(DefaultConverter).GetMethod(nameof(ToEnum), BindingFlags.NonPublic | BindingFlags.Static);
                return method.MakeGenericMethod(typeof(T)).CreateDelegate(typeof(ConverterFunc<T>), null);
            }

            Type otherType;
            if (typeof(T).IsArray)
            {
                otherType = typeof(T).GetElementType();
                var method = convertToArrayMethodInfo ??= typeof(DefaultConverter).GetMethod(nameof(ToArray), BindingFlags.NonPublic | BindingFlags.Static);
                return method.MakeGenericMethod(otherType).CreateDelegate(typeof(ConverterFunc<T>), null);
            }

            if (typeof(T).IsGenericType)
            {
                otherType = typeof(T).GetGenericTypeDefinition();
                if (typeof(IEnumerable<>) == otherType)
                {
                    otherType = typeof(T).GetGenericArguments()[0];
                    var method = convertToIEnumerableMethodInfo ??= typeof(DefaultConverter).GetMethod(nameof(ToIEnumerable), BindingFlags.NonPublic | BindingFlags.Static);
                    return method.MakeGenericMethod(otherType).CreateDelegate(typeof(ConverterFunc<T>), null);
                }
                if (typeof(List<>) == otherType)
                {
                    otherType = typeof(T).GetGenericArguments()[0];
                    var method = convertToListMethodInfo ??= typeof(DefaultConverter).GetMethod(nameof(ToList), BindingFlags.NonPublic | BindingFlags.Static);
                    return method.MakeGenericMethod(otherType).CreateDelegate(typeof(ConverterFunc<T>), null);
                }
                if (typeof(IReadOnlyList<>) == otherType)
                {
                    otherType = typeof(T).GetGenericArguments()[0];
                    var method = convertToIReadOnlyListMethodInfo ??= typeof(DefaultConverter).GetMethod(nameof(ToIReadOnlyList), BindingFlags.NonPublic | BindingFlags.Static);
                    return method.MakeGenericMethod(otherType).CreateDelegate(typeof(ConverterFunc<T>), null);
                }
                if (typeof(Collection<>) == otherType)
                {
                    otherType = typeof(T).GetGenericArguments()[0];
                    var method = convertToCollectionMethodInfo ??= typeof(DefaultConverter).GetMethod(nameof(ToCollection), BindingFlags.NonPublic | BindingFlags.Static);
                    return method.MakeGenericMethod(otherType).CreateDelegate(typeof(ConverterFunc<T>), null);
                }
                if (typeof(ICollection<>) == otherType)
                {
                    otherType = typeof(T).GetGenericArguments()[0];
                    var method = convertToICollectionMethodInfo ??= typeof(DefaultConverter).GetMethod(nameof(ToICollection), BindingFlags.NonPublic | BindingFlags.Static);
                    return method.MakeGenericMethod(otherType).CreateDelegate(typeof(ConverterFunc<T>), null);
                }
                if (typeof(IReadOnlyCollection<>) == otherType)
                {
                    otherType = typeof(T).GetGenericArguments()[0];
                    var method = convertToIReadOnlyCollectionMethodInfo ??= typeof(DefaultConverter).GetMethod(nameof(ToIReadOnlyCollection), BindingFlags.NonPublic | BindingFlags.Static);
                    return method.MakeGenericMethod(otherType).CreateDelegate(typeof(ConverterFunc<T>), null);
                }
                
                otherType = Nullable.GetUnderlyingType(typeof(T));
                if (otherType is Type && otherType.IsEnum)
                {
                    // We have to deal invoke this dynamically to work around the type constraint on Enum.TryParse.
                    var method = convertToNullableEnumMethodInfo ??= typeof(DefaultConverter).GetMethod(nameof(ToEnumNullable), BindingFlags.NonPublic | BindingFlags.Static);
                    return method.MakeGenericMethod(otherType).CreateDelegate(typeof(ConverterFunc<T>), null);
                }
            }

            return (ConverterFunc<T>)ToDefault;
        }

        private static bool ToDefault<T>(string value, CultureInfo culture, out T result)
        {
            result = default;
            return false;
        }

        private static bool ToString(string value, CultureInfo culture, out string result)
        {
            result = value;

            if (IsStringNullValid)
            {
                return true;
            }

            return value != null;
        }

        private static bool ToChar(string value, CultureInfo culture, out char result)
        {
            return char.TryParse(value, out result);
        }

        private static bool ToCharNullable(string value, CultureInfo culture, out char? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = char.TryParse(value, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToBool(string value, CultureInfo culture, out bool result)
        {
            return bool.TryParse(value, out result);
        }

        private static bool ToBoolNullable(string value, CultureInfo culture, out bool? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = bool.TryParse(value, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToByte(string value, CultureInfo culture, out byte result)
        {
            return byte.TryParse(value, IntegerNumberStyles, culture, out result);
        }

        private static bool ToByteNullable(string value, CultureInfo culture, out byte? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = byte.TryParse(value, IntegerNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToSByte(string value, CultureInfo culture, out sbyte result)
        {
            return sbyte.TryParse(value, IntegerNumberStyles, culture, out result);
        }

        private static bool ToSByteNullable(string value, CultureInfo culture, out sbyte? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = sbyte.TryParse(value, IntegerNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToShort(string value, CultureInfo culture, out short result)
        {
            return short.TryParse(value, IntegerNumberStyles, culture, out result);
        }

        private static bool ToShortNullable(string value, CultureInfo culture, out short? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = short.TryParse(value, IntegerNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToUShort(string value, CultureInfo culture, out ushort result)
        {
            return ushort.TryParse(value, IntegerNumberStyles, culture, out result);
        }

        private static bool ToUShortNullable(string value, CultureInfo culture, out ushort? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = ushort.TryParse(value, IntegerNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToInt(string value, CultureInfo culture, out int result)
        {
            return int.TryParse(value, IntegerNumberStyles, culture, out result);
        }

        private static bool ToIntNullable(string value, CultureInfo culture, out int? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = int.TryParse(value, IntegerNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToUInt(string value, CultureInfo culture, out uint result)
        {
            return uint.TryParse(value, IntegerNumberStyles, culture, out result);
        }

        private static bool ToUIntNullable(string value, CultureInfo culture, out uint? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = uint.TryParse(value, IntegerNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToLong(string value, CultureInfo culture, out long result)
        {
            return long.TryParse(value, IntegerNumberStyles, culture, out result);
        }

        private static bool ToLongNullable(string value, CultureInfo culture, out long? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = long.TryParse(value, IntegerNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToULong(string value, CultureInfo culture, out ulong result)
        {
            return ulong.TryParse(value, IntegerNumberStyles, culture, out result);
        }

        private static bool ToULongNullable(string value, CultureInfo culture, out ulong? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = ulong.TryParse(value, IntegerNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToFloat(string value, CultureInfo culture, out float result)
        {
            return float.TryParse(value, FloatingPointNumberStyles, culture, out result);
        }

        private static bool ToFloatNullable(string value, CultureInfo culture, out float? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = float.TryParse(value, FloatingPointNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToDouble(string value, CultureInfo culture, out double result)
        {
            return double.TryParse(value, FloatingPointNumberStyles, culture, out result);
        }

        private static bool ToDoubleNullable(string value, CultureInfo culture, out double? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = double.TryParse(value, FloatingPointNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToDecimal(string value, CultureInfo culture, out decimal result)
        {
            return decimal.TryParse(value, FloatingPointNumberStyles, culture, out result);
        }

        private static bool ToDecimalNullable(string value, CultureInfo culture, out decimal? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = decimal.TryParse(value, FloatingPointNumberStyles, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToDateTime(string value, CultureInfo culture, out DateTime result)
        {
            return DateTime.TryParse(value, culture, DateTimeStyles, out result);
        }

        private static bool ToDateTimeNullable(string value, CultureInfo culture, out DateTime? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = DateTime.TryParse(value, culture, DateTimeStyles, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToDateTimeOffset(string value, CultureInfo culture, out DateTimeOffset result)
        {
            return DateTimeOffset.TryParse(value, culture, DateTimeStyles, out result);
        }

        private static bool ToDateTimeOffsetNullable(string value, CultureInfo culture, out DateTimeOffset? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = DateTimeOffset.TryParse(value, culture, DateTimeStyles, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToGuidOffset(string value, CultureInfo culture, out Guid result)
        {
            return Guid.TryParse(value, out result);
        }

        private static bool ToGuidOffsetNullable(string value, CultureInfo culture, out Guid? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = Guid.TryParse(value, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToEnum<T>(string value, CultureInfo culture, out T result) 
            where T : struct, Enum
        {
            _ = culture;
            return Enum.TryParse(value, out result);
        }

        private static bool ToEnumNullable<T>(string value, CultureInfo culture, out T? result) 
            where T : struct, Enum
        {
            _ = culture;
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return true;
            }

            var parsed = Enum.TryParse<T>(value, out var converted);
            result = converted;
            return parsed;
        }

        private const char Separator = ',';
        private static bool ToArray<T>(string value, CultureInfo culture, out T[] result)
        {
            if (value is null)
            {
                result = null;
                return true;
            }

            if (value.Length == 0)
            {
                result = Array.Empty<T>();
                return true;
            }

            var values = value.Split(Separator);
            var convert = TypeConverter<T>.Convert;
            result = new T[values.Length];
            var converted = true;
            for (var i = 0; i < values.Length; i++)
            {
                converted &= convert(values[i], culture, out result[i]);
            }

            return converted;
        }

        private static bool ToIEnumerable<T>(string value, CultureInfo culture, out IEnumerable<T> result)
        {
            var parsed = ToArray<T>(value, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToICollection<T>(string value, CultureInfo culture, out ICollection<T> result)
        {
            var parsed = ToArray<T>(value, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToIReadOnlyCollection<T>(string value, CultureInfo culture, out IReadOnlyCollection<T> result)
        {
            var parsed = ToArray<T>(value, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToIReadOnlyList<T>(string value, CultureInfo culture, out IReadOnlyList<T> result)
        {
            var parsed = ToArray<T>(value, culture, out var converted);
            result = converted;
            return parsed;
        }

        private static bool ToCollection<T>(string value, CultureInfo culture, out Collection<T> result)
        {
            if (value is null)
            {
                result = null;
                return true;
            }

            if (value.Length == 0)
            {
                result = new Collection<T>();
                return true;
            }

            var values = value.Split(Separator);
            var convert = TypeConverter<T>.Convert;
            result = new Collection<T>(new List<T>(values.Length));
            var converted = true;
            for (var i = 0; i < values.Length; i++)
            {
                converted &= convert(values[i], culture, out var convertValue);
                result.Add(convertValue);
            }

            return converted;
        }

        private static bool ToList<T>(string value, CultureInfo culture, out List<T> result)
        {
            if (value is null)
            {
                result = null;
                return true;
            }

            if (value.Length == 0)
            {
                result = new List<T>();
                return true;
            }

            var values = value.Split(Separator);
            var convert = TypeConverter<T>.Convert;
            result = new List<T>(values.Length);
            var converted = true;
            for (var i = 0; i < values.Length; i++)
            {
                converted &= convert(values[i], culture, out var convertValue);
                result.Add(convertValue);
            }

            return converted;
        }
    }
}