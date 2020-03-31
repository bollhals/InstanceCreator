using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TechTalk.SpecFlow.Assist.InstanceBuilder
{
    public class TypeCreatorInfo
    {
        public Type Type { get; }

        public ConstructorOption[] ConstructorOptions { get; }

        public PropertyOption[] PropertyOptions { get; }

        public TypeCreatorInfo(Type type, ConstructorOption[] constructorOptions, PropertyOption[] propertyOptions)
        {
            Type = type;
            ConstructorOptions = constructorOptions;
            PropertyOptions = propertyOptions;
        }
    }

    public class PropertyOption
    {
        public string Name { get; }

        public PropertyInfo PropertyInfo { get; }

        public PropertyOption(string name, PropertyInfo propertyInfo)
        {
            Name = name;
            PropertyInfo = propertyInfo;
        }
    }

    public class ConstructorOption
    {
        public ConstructorInfo Constructor { get; }

        public ParameterOption[] Parameters { get; }

        public ConstructorOption(ConstructorInfo constructor, ParameterOption[] parameters)
        {
            Constructor = constructor;
            Parameters = parameters;
        }
    }

    public class ParameterOption
    {
        public string Name { get; }

        public ParameterInfo ParameterInfo { get; }

        public ParameterOption(string name, ParameterInfo parameterInfo)
        {
            Name = name;
            ParameterInfo = parameterInfo;
        }
    }
}
