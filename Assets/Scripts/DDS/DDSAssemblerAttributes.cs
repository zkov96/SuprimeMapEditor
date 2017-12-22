using System;

namespace DDS
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DDSAssemblerAttribute:Attribute
    {
        public readonly int width;
        public readonly int height;
        public readonly int dataLength;
        public readonly string type;
        
        public DDSAssemblerAttribute(int width, int height, int dataLength, string type)
        {
            this.width = width;
            this.height = height;
            this.dataLength = dataLength;
            this.type = type;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class DDSCompressorAttribute:Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class DDSDecompressorAttribute:Attribute
    {
    }
}