namespace InstanceCreator.Tests.Types
{
    public class ClassWithReadAndWritableProperties
    {
        public int Property1 { internal get; set; }
        public long Property2 { get; }
    }

    public struct StructWithReadAndWritableProperties
    {
        public int Property1 { get; }
        public long Property2 { internal get; set; }
    }
}