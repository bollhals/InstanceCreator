using System.Reflection;
using BenchmarkDotNet.Attributes;
using InstanceCreator.Creator;
using InstanceCreator.Tests.Types;

namespace InstanceCreator.Benchmarks.Creator
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreatorBenchmark
    {
        public CreatorBenchmark()
        {
            this.CtorStructConstructorInfo = typeof(StructWithTwoPropertiesAndConstructor).GetConstructors()[0];
            this.CtorStructConstructorParameters = this.CtorStructConstructorInfo.GetParameters();
            this.PropStructConstructorInfo = null;
            this.PropStructProperties = typeof(StructWithTwoProperties).GetProperties();
            this.CtorClassConstructorInfo = typeof(ClassWithTwoPropertiesAndConstructor).GetConstructors()[0];
            this.CtorClassConstructorParameters = this.CtorClassConstructorInfo.GetParameters();
            this.PropClassConstructorInfo = typeof(ClassWithTwoProperties).GetConstructors()[0];
            this.PropClassProperties = typeof(ClassWithTwoProperties).GetProperties();
        }

        public ConstructorInfo CtorStructConstructorInfo { get; set; }
        public ParameterInfo[] CtorStructConstructorParameters { get; set; }

        public ConstructorInfo PropStructConstructorInfo { get; set; }
        public PropertyInfo[] PropStructProperties { get; set; }

        public ConstructorInfo CtorClassConstructorInfo { get; set; }
        public ParameterInfo[] CtorClassConstructorParameters { get; set; }

        public ConstructorInfo PropClassConstructorInfo { get; set; }
        public PropertyInfo[] PropClassProperties { get; set; }
        
        [Benchmark]
        public void Ctor_Struct()
        {
            CreatorCompiler.CompileConstructorInjection<StructWithTwoPropertiesAndConstructor>(this.CtorStructConstructorInfo, this.CtorStructConstructorParameters);
        }
        
        [Benchmark]
        public void Ctor_Class()
        {
            CreatorCompiler.CompileConstructorInjection<ClassWithTwoPropertiesAndConstructor>(this.CtorClassConstructorInfo, this.CtorClassConstructorParameters);
        }

        [Benchmark]
        public void Prop_Struct()
        {
            CreatorCompiler.CompilePropertyInjection<StructWithTwoProperties>(this.PropStructConstructorInfo, this.PropStructProperties);
        }

        [Benchmark]
        public void Prop_Class()
        {
            CreatorCompiler.CompilePropertyInjection<ClassWithTwoProperties>(this.PropClassConstructorInfo, this.PropClassProperties);
        }
    }
}