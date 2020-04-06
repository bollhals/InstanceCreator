namespace InstanceCreator.Tests.Types
{
    public class ClassWithDefaultParameter
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }

        public ClassWithDefaultParameter(int property1, long property2 = long.MaxValue)
        {
            Property1 = property1;
            Property2 = property2;
        }
    }
    
    public struct StructWithDefaultParameter
    {
        public int Property1 { get; set; }
        public long Property2 { get; set; }

        public StructWithDefaultParameter(int property1, long property2 = long.MaxValue)
        {
            Property1 = property1;
            Property2 = property2;
        }
    }
    
    public class ClassWithOnlyDefaultParameter
    {
        public byte PropertyByte { get; }
        public sbyte PropertySByte { get; }
        public short PropertyShort { get; }
        public ushort PropertyUShort { get; }
        public int PropertyInt { get; }
        public uint PropertyUInt { get; }
        public long PropertyLong { get; }
        public ulong PropertyULong { get; }
        public float PropertyFloat { get; }
        public double PropertyDouble { get; }
        public char PropertyChar { get; }
        public string PropertyString { get; }
        public object PropertyObject { get; }

        public ClassWithOnlyDefaultParameter(byte propertyByte = byte.MaxValue, sbyte propertySByte = sbyte.MaxValue, short propertyShort = short.MaxValue, ushort propertyUShort = ushort.MaxValue,
            int propertyInt = int.MaxValue, uint propertyUInt = uint.MaxValue, long propertyLong = long.MaxValue, ulong propertyULong = ulong.MaxValue, 
            float propertyFloat = float.MaxValue, double propertyDouble = double.MaxValue, char propertyChar = char.MaxValue, string propertyString = "Test", object propertyObject = null)
        {
            PropertyByte = propertyByte;
            PropertySByte = propertySByte;
            PropertyShort = propertyShort;
            PropertyUShort = propertyUShort;
            PropertyInt = propertyInt;
            PropertyUInt = propertyUInt;
            PropertyLong = propertyLong;
            PropertyULong = propertyULong;
            PropertyFloat = propertyFloat;
            PropertyDouble = propertyDouble;
            PropertyChar = propertyChar;
            PropertyString = propertyString;
            PropertyObject = propertyObject;
        }
    }
}