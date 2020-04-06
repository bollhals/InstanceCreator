namespace InstanceCreator.Tests.Types
{
    public class ClassWithNoPublicConstructors
    {
        public int Property1 { get; set; }

        private ClassWithNoPublicConstructors()
        { }
    }
}