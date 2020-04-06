using System.Globalization;

namespace InstanceCreator.Converter
{
    public delegate bool ConverterFunc<T>(string obj, CultureInfo culture, out T value);

    public static class TypeConverter<T>
    {
        private static ConverterFunc<T> cache;

        public static ConverterFunc<T> Convert
        {
            get => cache ??= (ConverterFunc<T>)DefaultConverter.Get<T>();
            set => cache = value;
        }
    }
}
