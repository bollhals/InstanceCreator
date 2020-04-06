using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TechTalk.SpecFlow;

namespace InstanceCreator.Creator
{
    public static class CreatorExtensions
    {
        public static List<T> ToSet<T>(this Table table, CultureInfo cultureInfo = null)
        {
            var headerArray = (string[])table.Header;
            EnsureHorizontalTable(table, headerArray);
            var typeCreationInfo = TypeCreationInfoCache.Get(typeof(T));
            var headerForConstructor= GetHeadersForConstructorMatch(headerArray);
            var constructor = GetBestConstructorFor(typeCreationInfo.ConstructorOptions, headerForConstructor);

            cultureInfo ??= CultureInfo.InvariantCulture;
            var ctorFunc = constructor.GetFunc<T>();
            var result = new List<T>(table.Rows.Count);
            for (var index = 0; index < table.Rows.Count; index++)
            {
                var (values, tables) = constructor.IsDefaultConstructor ? GetParameterArraysFor(typeCreationInfo.PropertyOptions, headerForConstructor, headerArray, table.Rows[index]) : GetParameterArraysFor(constructor.Parameters, headerForConstructor, headerArray, table.Rows[index]);
                result.Add(ctorFunc.Invoke(values, tables, cultureInfo));
            }

            return result;
        }

        public static T ToInstance<T>(this Table table, CultureInfo cultureInfo = null)
        {
            table = AdjustTableToHorizontal(table);
            var headerArray = (string[])table.Header;
            var typeCreationInfo = TypeCreationInfoCache.Get(typeof(T));
            var headerForConstructor= GetHeadersForConstructorMatch(headerArray);
            var constructor = GetBestConstructorFor(typeCreationInfo.ConstructorOptions, headerForConstructor);
            var (values, tables) = constructor.IsDefaultConstructor ? GetParameterArraysFor(typeCreationInfo.PropertyOptions, headerForConstructor, headerArray, table.Rows[0]) : GetParameterArraysFor(constructor.Parameters, headerForConstructor, headerArray, table.Rows[0]);
            return constructor.GetFunc<T>().Invoke(values, tables, cultureInfo ?? CultureInfo.InvariantCulture);
        }

        private static void EnsureHorizontalTable(Table table, string[] headerArray)
        {
            if (IsVerticalTable(headerArray))
            {
                throw new ArgumentException("Table needs to a be horizontal.");
            }

            if (table.RowCount < 1)
            {
               throw new ArgumentException("Table does not have any rows.");
            }
        }

        private static Table AdjustTableToHorizontal(Table table)
        {
            if (IsVerticalTable((string[])table.Header))
            {
                return TransformToHorizontalTable(table);
            }

            if (table.RowCount == 1)
            {
                return table;
            }

            throw new ArgumentException($"Table does not have 1 row. (Has {table.RowCount})");
        }

        private static Table TransformToHorizontalTable(Table table)
        {
            var tmpHeaders = new string[table.RowCount];
            var tmpValues = new string[table.RowCount];
            for (var index = 0; index < table.Rows.Count; index++)
            {
                var row = table.Rows[index];
                tmpHeaders[index] = row[0];
                tmpValues[index] = row[1];
            }

            var tmpTable = new Table(tmpHeaders);
            tmpTable.AddRow(tmpValues);
            return tmpTable;
        }

        private static bool IsVerticalTable(string[] headerArray)
        {
            return headerArray.Length == 2 && headerArray[0] == "Field" && headerArray[1] == "Value";
        }

        private static string[] GetHeadersForConstructorMatch(string[] headerArray)
        {
            // Split of parameters for inner class. (All that contain '.')
            var result = headerArray;
            bool first = true;
            for (var i = 0; i < headerArray.Length; i++)
            {
                var indexOf = headerArray[i].IndexOf('.');
                if (indexOf > 0)
                {
                    if (first)
                    {
                        first = false;
                        result = new string[headerArray.Length];
                        Array.Copy(headerArray, 0, result, 0, result.Length);
                    }

                    result[i] = headerArray[i].Substring(0, indexOf);
                }
            }

            return result;
        }

        private static ConstructorOption GetBestConstructorFor(ConstructorOption[] constructorOptions, string[] names)
        {
            // Get the first constructor that matches all parameters of the table
            foreach (var constructor in constructorOptions)
            {
                if (names.ContainsAll(constructor.Parameters))
                {
                    return constructor;
                }
            }

            return constructorOptions[0];
        }

        private static bool ContainsAll(this string[] names, ParameterInfo[] parameters)
        {
            if (parameters is null)
            {
                return true;
            }

            foreach (string name in names)
            {
                bool match = false;
                foreach (var parameter in parameters)
                {
                    if (string.Equals(name, parameter.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        match = true;
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

        private static readonly Table[] TableCache = new Table[16];

        private static (string[], Table[]) GetParameterArraysFor(PropertyInfo[] propertyInfos, string[] headerForConstructor, string[] tableHeader, TableRow row)
        {
            // Property Injection
            var length = propertyInfos.Length;
            var result = new string[length];
            if (headerForConstructor == tableHeader)
            {
                // No inner class, use TableCache as cache, as it's not used
                for (var i = 0; i < length; i++)
                {
                    // Map the table values to constructor parameter order
                    var headerIndex = headerForConstructor.GetIndexFor(propertyInfos[i].Name);
                    if (headerIndex >= 0)
                    {
                        result[i] = row[headerIndex];
                    }
                }

                return (result, TableCache);
            }
            
            // Split of values for inner classes
            var tables = new Table[length];
            for (var i = 0; i < length; i++)
            {
                var headerIndex = tableHeader.GetIndexFor(propertyInfos[i].Name);
                if (headerIndex != -1)
                {
                    // found in original, so can't be for inner class
                    result[i] = row[headerIndex];
                    continue;
                }
                    
                headerIndex = headerForConstructor.GetIndexFor(propertyInfos[i].Name);
                if (headerIndex != -1)
                {
                    // not found in original but in headerForConstructor => modified for sub class
                    var subHeader = new string[headerForConstructor.CountSameValue(headerIndex)];
                    var rowValues = new string[subHeader.Length];
                    var compareValue = headerForConstructor[headerIndex];
                    int count = 0;
                    for (int j = headerIndex; count < subHeader.Length; j++)
                    {
                        if (compareValue == headerForConstructor[j])
                        {
                            // Substring the inner class parameter name and assign the value
                            subHeader[count] = tableHeader[j].Substring(compareValue.Length + 1);
                            rowValues[count++] = row[j];
                        }
                    }
                    tables[i] = new Table(subHeader);
                    tables[i].AddRow(rowValues);
                }
            }

            return (result, tables);
        }

        private static (string[], Table[]) GetParameterArraysFor(ParameterInfo[] constructorParameters, string[] headerForConstructor, string[] tableHeader, TableRow row)
        {
            // Constructor Injection
            var length = constructorParameters.Length;
            var result = new string[length];
            
            if (headerForConstructor == tableHeader)
            {
                // No inner class, use TableCache as cache, as it's not used
                for (var i = 0; i < length; i++)
                {
                    // Map the table values to constructor parameter order
                    var headerIndex = headerForConstructor.GetIndexFor(constructorParameters[i].Name);
                    if (headerIndex >= 0)
                    {
                        result[i] = row[headerIndex];
                    }
                }

                return (result, TableCache);
            }

            // Split of values for inner classes
            var tables = new Table[length];
            for (var i = 0; i < length; i++)
            {
                var headerIndex = tableHeader.GetIndexFor(constructorParameters[i].Name);
                if (headerIndex != -1)
                {
                    // found in original, so can't be for inner class
                    result[i] = row[headerIndex];
                    continue;
                }
                    
                headerIndex = headerForConstructor.GetIndexFor(constructorParameters[i].Name);
                if (headerIndex != -1)
                {
                    // not found in original but in headerForConstructor => modified for sub class
                    var subHeader = new string[headerForConstructor.CountSameValue(headerIndex)];
                    var rowValues = new string[subHeader.Length];
                    var compareValue = headerForConstructor[headerIndex];
                    int count = 0;
                    for (int j = headerIndex; count < subHeader.Length; j++)
                    {
                        if (compareValue == headerForConstructor[j])
                        {
                            // Substring the inner class parameter name and assign the value
                            subHeader[count] = tableHeader[j].Substring(compareValue.Length + 1);
                            rowValues[count++] = row[j];
                        }
                    }
                    tables[i] = new Table(subHeader);
                    tables[i].AddRow(rowValues);
                }
            }

            return (result, tables);
        }

        private static int CountSameValue(this string[] array, int start)
        {
            int count = 1;
            var compareValue = array[start];
            for (int i = start + 1; i < array.Length; i++)
            {
                if (compareValue == array[i])
                {
                    count++;
                }
            }

            return count;
        }

        private static int GetIndexFor(this string[] array, string value)
        {
            for (var i = 0; i < array.Length; i++)
            {
                if (string.Equals(array[i], value, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}