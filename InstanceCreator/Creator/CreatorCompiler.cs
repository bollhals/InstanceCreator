using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using InstanceCreator.Converter;
using TechTalk.SpecFlow;

namespace InstanceCreator.Creator
{
    internal static class CreatorCompiler
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> CacheProperty = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> CacheDelegateInvoke = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly MethodInfo ToInstanceMethodInfo = typeof(CreatorExtensions).GetMethod(nameof(CreatorExtensions.ToInstance), BindingFlags.Public | BindingFlags.Static);
        private static readonly Type[] MethodParameterTypes = { typeof(string[]), typeof(Table[]), typeof(CultureInfo) };

        public static Func<string[], Table[], CultureInfo, T> CompilePropertyInjection<T>(ConstructorInfo constructor, PropertyInfo[] properties)
        {
            /* Compiles a method looking like this: (example with 2 parameters)
             *
             * private static AClassWithProperties CreateInstanceThroughProp(string[] input, Table[] tables, CultureInfo cultureInfo)
             * {
             *     var result = new AClassWithProperties();
             *     
             *     // NoTableCreationTypes
             *     TypeConverter<int>.Convert.Invoke(input[0], cultureInfo, out var parameter_0);
             *     result.Property0 = parameter_0;
             *     
             *     // TableCreationTypes
             *     if (tables[1] is null)
             *     {
             *         TypeConverter<AType>.Convert.Invoke(input[1], cultureInfo, out var parameter_1);
             *     }
             *     else
             *     {
             *         parameter_1 = tables[1].ToInstance<AType>();
             *     }
             *     result.Property1 = parameter_1;
             *
             *     return result;
             * }
             */
            var method = new DynamicMethod("CreateInstanceThroughProp", typeof(T), MethodParameterTypes, typeof(CreatorCompiler));
            var generator = method.GetILGenerator();

            // var result = new T();
            var instanceVariable = generator.DeclareLocal(typeof(T));
            if (constructor != null)
            {
                // Normal ctor
                generator.Emit(OpCodes.Newobj, constructor);
                generator.EmitStoreInLocal(instanceVariable);
            }
            else
            {
                // Default ctor struct, no action needed since we set all properties
            }

            for (var i = 0; i < properties.Length; i++)
            {
                var propertyType = properties[i].PropertyType;
                var localVariable = generator.DeclareLocal(propertyType);
                if (IsNoTableCreationType(propertyType))
                {
                    generator.EmitTypeConverterCall(i, localVariable, null);
                }
                else
                {
                    generator.EmitToInstanceCall(i, localVariable, null);
                }

                // Assign to property => result.Property_i =
                if (constructor != null)
                {
                    // Normal assignment
                    generator.EmitLoadLocal(instanceVariable);
                    generator.EmitLoadLocal(localVariable);
                    generator.Emit(OpCodes.Callvirt, properties[i].GetSetMethod());
                }
                else
                {
                    // struct assignment for default ctor structs
                    generator.Emit(OpCodes.Ldloca_S, instanceVariable);
                    generator.EmitLoadLocal(localVariable);
                    generator.Emit(OpCodes.Call, properties[i].GetSetMethod());
                }
            }

            // return result;
            generator.EmitLoadLocal(instanceVariable);
            generator.Emit(OpCodes.Ret);

            return (Func<string[], Table[], CultureInfo, T>) method.CreateDelegate(typeof(Func<string[], Table[], CultureInfo, T>));
        }

        public static Func<string[], Table[], CultureInfo, T> CompileConstructorInjection<T>(ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            /* Compiles a method looking like this: (example with 2 parameters)
             *
             * private static AClassWithAConstructor CreateInstanceOf(string[] input, Table[] tables, CultureInfo cultureInfo)
             * {
             *     // NoTableCreationTypes
             *     TypeConverter<int>.Convert.Invoke(input[0], cultureInfo, out var parameter_0);
             *
             *     // TableCreationTypes
             *     if (tables[1] is null)
             *     {
             *         TypeConverter<AType>.Convert.Invoke(input[1], cultureInfo, out var parameter_1);
             *     }
             *     else
             *     {
             *         parameter_1 = tables[1].ToInstance<AType>();
             *     }
             *
             *     return new AClassWithAConstructor(parameter_0, parameter_1);
             * }
             */
            var method = new DynamicMethod("CreateInstanceOf", typeof(T), MethodParameterTypes);
            var generator = method.GetILGenerator();

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterInfo = parameters[i];
                var localVariable = generator.DeclareLocal(parameterInfo.ParameterType);
                if (IsNoTableCreationType(parameterInfo.ParameterType))
                {
                    generator.EmitTypeConverterCall(i, localVariable, parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null);
                }
                else
                {
                    generator.EmitToInstanceCall(i, localVariable, parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null);
                }
            }
            
            // Load all locals => parameter_X
            for (int i = 0; i < parameters.Length; i++)
            {
                generator.EmitLoadLocal(i);
            }

            // Call constructor => new T(parameter_X, parameter_X+1)
            generator.Emit(OpCodes.Newobj, constructor);
            // Return
            generator.Emit(OpCodes.Ret);
            return (Func<string[], Table[], CultureInfo, T>) method.CreateDelegate(typeof(Func<string[], Table[], CultureInfo, T>));
        }

        private static void EmitToInstanceCall(this ILGenerator generator, int parameterIndex, LocalBuilder localVariable, object defaultValue)
        { 
            /* Emits the following code:
             *
             * if (tables[1] is null)
             * {
             *     // From <EmitTypeConverterCall>
             *     TypeConverter<AType>.Convert.Invoke(input[1], cultureInfo, out var parameter_1);
             * }
             * else
             * {
             *     parameter_1 = tables[1].ToInstance<AType>();
             * }
             */
            // check if table is null => if (inputTables[i] is null)
            generator.EmitLoadTableArg();
            generator.EmitLoadArrayElement(parameterIndex);
            var elseCase = generator.DefineLabel();
            var endIf = generator.DefineLabel();
            generator.Emit(OpCodes.Brtrue_S, elseCase);
            {
                generator.EmitTypeConverterCall(parameterIndex, localVariable, defaultValue);
                // go to end if
                generator.Emit(OpCodes.Br_S, endIf);
            }
            // else
            generator.MarkLabel(elseCase);
            {
                // load table => inputTables[i]
                generator.EmitLoadTableArg();
                generator.EmitLoadArrayElement(parameterIndex);
                // call ToInstance => .ToInstance<T>(cultureInfo)
                generator.EmitLoadCultureInfoArg();
                generator.Emit(OpCodes.Call, ToInstanceMethodInfo.MakeGenericMethod(localVariable.LocalType));
                // store in parameter_i => parameter_i =
                generator.EmitStoreInLocal(localVariable);
            }
            generator.MarkLabel(endIf);
        }

        private static void EmitTypeConverterCall(this ILGenerator generator, int parameterIndex, LocalBuilder localVariable, object defaultValue)
        {
            /* Emits the following code:
             *
             * // When default value is null
             * TypeConverter<int>.Convert.Invoke(input[0], cultureInfo, out var parameter_0);
             *
             * // When default value is not null
             * if (TypeConverter<int>.Convert.Invoke(input[0], cultureInfo, out var parameter_0) == false)
             * {
             *   parameter_0 = <default value of parameter>;
             * }
             */
            // Get converter => TypeConverter<X>.Convert
            generator.Emit(OpCodes.Call, CacheProperty.GetOrAdd(localVariable.LocalType, t => typeof(TypeConverter<>).MakeGenericType(t).GetProperty(nameof(TypeConverter<int>.Convert), BindingFlags.Public | BindingFlags.Static).GetMethod));
            
            // Load first parameter & index => input[parameterIndex]
            generator.EmitLoadInputArg();
            generator.EmitLoadArrayElement(parameterIndex);
            // Load 2nd parameter => cultureInfo
            generator.EmitLoadCultureInfoArg();
            // Load 3rd parameter => out parameter_X
            generator.Emit(OpCodes.Ldloca_S, localVariable);
            // do the call => .Invoke(input[parameterIndex], cultureInfo, out var parameter_X);
            generator.Emit(OpCodes.Callvirt, CacheDelegateInvoke.GetOrAdd(localVariable.LocalType, t => typeof(ConverterFunc<>).MakeGenericType(t).GetMethod(nameof(ConverterFunc<int>.Invoke), BindingFlags.Public | BindingFlags.Instance)));

            if (defaultValue is null)
            {
                // Drop return value (bool)
                generator.Emit(OpCodes.Pop);
            }
            else
            {
                // if (TypeConverter<T>... == false)
                var endIfLabel = generator.DefineLabel();
                generator.Emit(OpCodes.Brtrue_S, endIfLabel);

                // { parameter_i = <default value of parameter>; }
                generator.EmitLoadValue(defaultValue);
                generator.EmitStoreInLocal(localVariable);
                generator.MarkLabel(endIfLabel);
            }
        }
        
        private static void EmitLoadValue(this ILGenerator generator, object value)
        {
            if (value is int intValue)
            {
                generator.Emit(OpCodes.Ldc_I4, intValue);
            }
            else if (value is long longValue)
            {
                generator.Emit(OpCodes.Ldc_I8, longValue);
            }
            else if (value is short shortValue)
            {
                generator.Emit(OpCodes.Ldc_I4, (int)shortValue);
            }
            else if (value is byte byteValue)
            {
                generator.Emit(OpCodes.Ldc_I4_S, byteValue);
            }
            else if (value is uint uintValue)
            {
                generator.Emit(OpCodes.Ldc_I4, (int)uintValue);
            }
            else if (value is ulong ulongValue)
            {
                generator.Emit(OpCodes.Ldc_I8, (long)ulongValue);
            }
            else if (value is ushort ushortValue)
            {
                generator.Emit(OpCodes.Ldc_I4, (int)ushortValue);
            }
            else if (value is sbyte sbyteValue)
            {
                generator.Emit(OpCodes.Ldc_I4_S, (byte)sbyteValue);
            } 
            else if (value is float floatValue)
            {
                generator.Emit(OpCodes.Ldc_R4, floatValue);
            } 
            else if (value is double doubleValue)
            {
                generator.Emit(OpCodes.Ldc_R8, doubleValue);
            }
            else if (value is char charValue)
            {
                generator.Emit(OpCodes.Ldc_I4, (int)charValue);
            }
            else if (value is string stringValue)
            {
                generator.Emit(OpCodes.Ldstr, stringValue);
            }
            else
            {
                generator.Emit(OpCodes.Ldnull);
            }
        }

        private static void EmitLoadLocal(this ILGenerator generator, int i)
        {
            switch (i)
            {
                case 0: generator.Emit(OpCodes.Ldloc_0); return;
                case 1: generator.Emit(OpCodes.Ldloc_1); return;
                case 2: generator.Emit(OpCodes.Ldloc_2); return;
                case 3: generator.Emit(OpCodes.Ldloc_3); return;
                default: generator.Emit(OpCodes.Ldloc_S, (byte) i); return;
            }
        }

        private static void EmitLoadLocal(this ILGenerator generator, LocalBuilder local)
        {
            generator.Emit(OpCodes.Ldloc, local);
        }

        private static void EmitStoreInLocal(this ILGenerator generator, LocalBuilder local)
        {
            generator.Emit(OpCodes.Stloc, local);
        }

        private static void EmitLoadInputArg(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldarg_0);
        }

        private static void EmitLoadTableArg(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldarg_1);
        }

        private static void EmitLoadCultureInfoArg(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldarg_2);
        }

        private static void EmitLoadArrayElement(this ILGenerator generator, int index)
        {
            switch (index)
            {
                case 0: generator.Emit(OpCodes.Ldc_I4_0); break;
                case 1: generator.Emit(OpCodes.Ldc_I4_1); break;
                case 2: generator.Emit(OpCodes.Ldc_I4_2); break;
                case 3: generator.Emit(OpCodes.Ldc_I4_3); break;
                case 4: generator.Emit(OpCodes.Ldc_I4_4); break;
                case 5: generator.Emit(OpCodes.Ldc_I4_5); break;
                case 6: generator.Emit(OpCodes.Ldc_I4_6); break;
                case 7: generator.Emit(OpCodes.Ldc_I4_7); break;
                case 8: generator.Emit(OpCodes.Ldc_I4_8); break;
                default: generator.Emit(OpCodes.Ldc_I4_S, (byte)index); break;
            }
            generator.Emit(OpCodes.Ldelem_Ref);
        }

        private static bool IsNoTableCreationType(Type parameterType)
        {
            return parameterType.IsPrimitive || 
                   parameterType == typeof(string) || 
                   parameterType.IsEnum || 
                   parameterType.IsArray ||
                   parameterType == typeof(decimal) || 
                   parameterType == typeof(DateTime) ||
                   parameterType == typeof(DateTimeOffset) ||
                   parameterType == typeof(Guid) ||
                   parameterType.IsGenericType && IsNoTableCreationTypeGeneric(parameterType.GetGenericTypeDefinition(), parameterType.GetGenericArguments()[0]);
        }

        private static bool IsNoTableCreationTypeGeneric(Type openGenericType, Type genericType)
        {
            return openGenericType == typeof(IEnumerable<>) ||
                   openGenericType == typeof(List<>) ||
                   openGenericType == typeof(IReadOnlyList<>) ||
                   openGenericType == typeof(Nullable<>) && IsNoTableCreationTypeNullableGeneric(genericType) ||
                   openGenericType == typeof(IList<>) ||
                   openGenericType == typeof(IReadOnlyCollection<>) ||
                   openGenericType == typeof(ICollection<>);
        }

        private static bool IsNoTableCreationTypeNullableGeneric(Type genericType)
        {
            return genericType.IsPrimitive ||
                   genericType == typeof(decimal) || 
                   genericType == typeof(DateTime) ||
                   genericType == typeof(DateTimeOffset) ||
                   genericType == typeof(Guid);
        }
    }
}