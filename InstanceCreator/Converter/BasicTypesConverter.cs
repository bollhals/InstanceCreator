using System;

namespace InstanceCreator.Converter
{
    public class BasicTypesConverter
    {
        public bool TryConvert(string value, Type type, out object result)
        {
            if (type == typeof(string))
            {
                result = value;
                return true;
            }

            if (type.IsPrimitive || type == typeof(decimal))
            {
                return ParseWithChangeType(value, type, out result);
            }

            if (type.IsEnum)
            {
                return Enum.TryParse(type, value, true, out result);
            }

            if (type == typeof(DateTime))
            {
                var converted = DateTime.TryParse(value, out var tmp);
                result = tmp;
                return converted;
            }

            if (type == typeof(Guid))
            {
                var converted = Guid.TryParse(value, out var tmp);
                result = tmp;
                return converted;
            }

            type = Nullable.GetUnderlyingType(type);
            if (type != null)
            {
                // nullable type, check value
                if (string.IsNullOrEmpty(value))
                {
                    result = null;
                    return true;
                }
                return this.TryConvert(value, type, out result);
            }

            result = null;
            return false;
        }

        private static bool ParseWithChangeType(string value, Type type, out object result)
        {
            try
            {
                result = Convert.ChangeType(value, type);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}