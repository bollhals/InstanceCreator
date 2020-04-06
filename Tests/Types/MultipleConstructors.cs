namespace InstanceCreator.Tests.Types
{
    public class ClassWithMultipleConstructors
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }
        public bool Property3 { get; set; }

        public ClassWithMultipleConstructors()
        { }

        public ClassWithMultipleConstructors(int property1)
        {
            Property1 = property1;
        }

        public ClassWithMultipleConstructors(int property1, long property2)
        {
            Property1 = property1;
            Property2 = property2;
        }
    }

    public struct StructWithMultipleConstructors
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }
        public bool Property3 { get; set; }
        
        public StructWithMultipleConstructors(int property1)
            : this()
        {
            Property1 = property1;
        }

        public StructWithMultipleConstructors(int property1, long property2)
            : this()
        {
            Property1 = property1;
            Property2 = property2;
        }
    }
}