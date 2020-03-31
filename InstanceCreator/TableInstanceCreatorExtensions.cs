using System;
using System.Reflection;

namespace TechTalk.SpecFlow.Assist.InstanceBuilder
{
    public static class TableInstanceCreatorExtensions
    {
        public static T Test<T>(Table table)
        {
            var typeCreatorInfo = typeof(T).BuildTypeCreatorInfo();

            var constructor = typeCreatorInfo.ConstructorOptions.GetBestConstructor(table.HeaderArray);
            var tableRow = table.Rows[0];
            var values = constructor.GetParameterValues(tableRow);

            var instance = constructor.Constructor.Invoke(values);

            if (constructor.Parameters.Length == 0)
            {
                FillInstanceProperties(typeCreatorInfo, constructor.Parameters, table.HeaderArray, tableRow, instance);
            }

            return (T)instance;
        }

        private static void FillInstanceProperties(TypeCreatorInfo typeCreatorInfo, ParameterOption[] constructorParameters, string[] headerArray, TableRow row, object instance)
        {
            for (var index = 0; index < headerArray.Length; index++)
            {
                string header = headerArray[index];
                if (constructorParameters.Contains(header))
                {
                    // Already covered
                    continue;
                }

                if (typeCreatorInfo.PropertyOptions.TryGetProperty(header, out var property))
                {
                    // assign value
                    // TODO convert value
                    property.SetValue(instance, row[index], null);
                }
            }
        }

        private static bool TryGetProperty(this PropertyOption[] propertyOptions, string name, out PropertyInfo info)
        {
            for (var i = 0; i < propertyOptions.Length; i++)
            {
                var propertyOption = propertyOptions[i];
                if (string.Equals(propertyOption.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    info = propertyOption.PropertyInfo;
                    return true;
                }
            }

            info = null;
            return false;
        }

        private static bool Contains(this ParameterOption[] constructorParameters, string propertyName)
        {
            for (var i = 0; i < constructorParameters.Length; i++)
            {
                if (string.Equals(constructorParameters[i].Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static object[] GetParameterValues(this ConstructorOption constructor, TableRow row)
        {
            var values = new object[constructor.Parameters.Length];
            for (var index = 0; index < constructor.Parameters.Length; index++)
            {
                var parameter = constructor.Parameters[index];
                var headerIndex = row.GetHeaderIndexFor(parameter.Name);
                
                if (headerIndex >= 0)
                {
                    var stringValue = row[headerIndex];
                    // TODO convert value
                    values[index] = stringValue;
                }
                else if (parameter.ParameterInfo.HasDefaultValue)
                {
                    values[index] = parameter.ParameterInfo.DefaultValue;
                }
                else
                {
                    values[index] = null;
                }
            }

            return values;
        }

        private static ConstructorOption GetBestConstructor(this ConstructorOption[] options, string[] names)
        {
            foreach (var option in options)
            {
                if (names.ContainsAll(option.Parameters))
                {
                    return option;
                }
            }

            throw new InvalidOperationException("No constructor found to use");
        }

        private static bool ContainsAll(this string[] names, ParameterOption[] parameters)
        {
            if (parameters.Length == 0)
            {
                return true;
            }
            foreach (string name in names)
            {
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                bool match = false;
                foreach (var parameter in parameters)
                {
                    match = string.Equals(name, parameter.Name, StringComparison.OrdinalIgnoreCase);
                    if (match)
                    {
                        break;    
                    }
                }

                if (!match)
                {
                    return false;
                }
            }

            return true;
        }

        private static TypeCreatorInfo BuildTypeCreatorInfo(this Type type)
        {
            var constructorInfos = type.GetConstructors();
            var constructorOptions = new ConstructorOption[constructorInfos.Length];
            for (var i = 0; i < constructorInfos.Length; i++)
            {
                constructorOptions[i] = constructorInfos[i].BuildConstructorOption();
            }

            var propertyInfos = type.GetProperties();
            var propertyOptions = new PropertyOption[propertyInfos.Length];
            for (var i = 0; i < propertyInfos.Length; i++)
            {
                var propertyInfo = propertyInfos[i];
                // TODO Sanitize name
                propertyOptions[i] = new PropertyOption(propertyInfo.Name, propertyInfo);
            }

            return new TypeCreatorInfo(type, constructorOptions, propertyOptions);
        }

        private static ConstructorOption BuildConstructorOption(this ConstructorInfo constructor)
        {
            var parameterInfos = constructor.GetParameters();
            var parameters = new ParameterOption[parameterInfos.Length];
            for (var i = 0; i < parameterInfos.Length; i++)
            {
                var parameterInfo = parameterInfos[i];
                // TODO Sanitize name
                parameters[i] = new ParameterOption(parameterInfo.Name, parameterInfo);
            }
            return new ConstructorOption(constructor, parameters);
        }
    }
}
