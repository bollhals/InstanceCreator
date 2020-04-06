namespace InstanceCreator.Tests.Types
{
    public class ClassWithTwoProperties
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }
    }

    public class ClassWithTwoPropertiesAndConstructor : ClassWithTwoProperties
    {
        public ClassWithTwoPropertiesAndConstructor(int property1, long property2)
        {
            this.Property1 = property1;
            this.Property2 = property2;
        }
    }

    public struct StructWithTwoProperties
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }
    }

    public struct StructWithTwoPropertiesAndConstructor
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }

        public StructWithTwoPropertiesAndConstructor(int property1, long property2)
        {
            Property1 = property1;
            Property2 = property2;
        }
    }
}