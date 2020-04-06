using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TechTalk.SpecFlow;

namespace InstanceCreator.Creator
{
    internal static class TypeCreationInfoCache
    {
        private static readonly ConcurrentDictionary<Type, TypeCreationInfo> Cache = new ConcurrentDictionary<Type, TypeCreationInfo>();
        private static readonly Func<Type, TypeCreationInfo> CreateTypeCreationInfoAction = t => Create(t);

        public static TypeCreationInfo Get(Type type)
        {
            return Cache.GetOrAdd(type, CreateTypeCreationInfoAction);
        }

        private static TypeCreationInfo Create(Type type)
        {
            PropertyInfo[] properties = null;
            var constructorInfos = type.GetConstructors();
            ConstructorOption[] options;

            if (type.IsValueType)
            {
                // add default struct ctor
                options = new ConstructorOption[constructorInfos.Length + 1];
                properties = GetWritableProperties(type.GetProperties());
                options[^1] = new ConstructorOption(null, properties);
            }
            else
            {
                if (constructorInfos.Length == 0)
                {
                    throw new InvalidOperationException($"No suitable constructor found on type {type.FullName}");
                }
                options = new ConstructorOption[constructorInfos.Length];
            }

            for (var i = 0; i < constructorInfos.Length; i++)
            {
                var constructorInfo = constructorInfos[i];
                var parameters = constructorInfo.GetParameters();

                if (parameters.Length == 0)
                {
                    properties ??= GetWritableProperties(type.GetProperties());
                    options[i] = new ConstructorOption(constructorInfo, properties);
                }
                else
                {
                    options[i] = new ConstructorOption(constructorInfo, parameters);
                }
            }

            // Sort smallest one first => map all provided names to the ctor with the most matches
            Array.Sort(options, (x, y) =>
            {
                if (x.Parameters is null)
                {
                    return 1;
                }

                if (y.Parameters is null)
                {
                    return -1;
                }
                return x.Parameters.Length.CompareTo(y.Parameters.Length);
            });

            return new TypeCreationInfo(type, options, properties);
        }

        private static PropertyInfo[] GetWritableProperties(PropertyInfo[] properties)
        {
            var count = properties.Count(p => !p.CanWrite);
            if (count > 0)
            {
                var tmp = new PropertyInfo[properties.Length - count];
                int i = 0;
                foreach (var prop in properties)
                {
                    if (prop.CanWrite)
                    {
                        tmp[i++] = prop;
                    }
                }

                properties = tmp;
            }

            return properties;
        }
    }

    internal sealed class TypeCreationInfo
    {
        public Type Type { get; }

        public ConstructorOption[] ConstructorOptions { get; }

        public PropertyInfo[] PropertyOptions { get; }

        public TypeCreationInfo(Type type, ConstructorOption[] constructorOptions, PropertyInfo[] propertyOptions)
        {
            Type = type;
            ConstructorOptions = constructorOptions;
            PropertyOptions = propertyOptions;
        }
    }

    internal sealed class ConstructorOption
    {
        private Delegate constructorFunc;

        public ConstructorInfo Constructor { get; }
        public ParameterInfo[] Parameters { get; }
        public PropertyInfo[] Properties { get; }

        public bool IsDefaultConstructor => this.Parameters is null;

        public ConstructorOption(ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            Constructor = constructor;
            Parameters = parameters;
        }

        public ConstructorOption(ConstructorInfo constructor, PropertyInfo[] properties)
        {
            Constructor = constructor;
            Properties = properties;
        }

        public Func<string[], Table[], CultureInfo, T> GetFunc<T>()
        {
            var func = this.constructorFunc;
            if (!(func is null))
            {
                return (Func<string[], Table[], CultureInfo, T>) func;
            }

            this.constructorFunc = func = this.IsDefaultConstructor ? 
                CreatorCompiler.CompilePropertyInjection<T>(this.Constructor, this.Properties) : 
                CreatorCompiler.CompileConstructorInjection<T>(this.Constructor, this.Parameters);

            return (Func<string[], Table[], CultureInfo, T>) func;
        }
    }
}