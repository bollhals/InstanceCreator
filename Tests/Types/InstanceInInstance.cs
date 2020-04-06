namespace InstanceCreator.Tests.Types
{
    public class ClassInClass
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }

        public ClassWithTwoProperties Inner { get; set; }
    }

    public class ClassInClassWithConstructor
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }

        public ClassWithTwoProperties Inner { get; set; }

        public ClassInClassWithConstructor(int property1, long property2, ClassWithTwoProperties inner)
        {
            Property1 = property1;
            Property2 = property2;
            Inner = inner;
        }
    }
    
    public struct StructInStruct
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }
        public StructWithTwoProperties Inner { get; set; }
    }

    public struct StructInStructWithConstructor
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }
        public StructWithTwoPropertiesAndConstructor Inner { get; set; }

        public StructInStructWithConstructor(int property1, long property2, StructWithTwoPropertiesAndConstructor inner)
        {
            Property1 = property1;
            Property2 = property2;
            Inner = inner;
        }
    }
}