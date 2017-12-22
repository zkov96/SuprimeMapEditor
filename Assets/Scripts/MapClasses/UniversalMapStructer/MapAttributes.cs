using System;
using System.Reflection;

namespace MapClasses.UniversalMapStructer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapAttribute : Attribute
    {
        public int version;
        public MapAttribute(int version)
        {
            this.version = version;
        }
    }

//    [AttributeUsage(AttributeTargets.Field)] public class MapHeaderFieldAttribute : Attribute {}
//    [AttributeUsage(AttributeTargets.Field)] public class TerrainFieldAttribute : Attribute {}
//    [AttributeUsage(AttributeTargets.Field)] public class LightingSettingsFieldAttribute : Attribute {}
//    [AttributeUsage(AttributeTargets.Field)] public class FogSettingsFieldAttribute : Attribute {}
//    [AttributeUsage(AttributeTargets.Field)] public class WaterSettingsFieldAttribute : Attribute {}
    
    [AttributeUsage(AttributeTargets.Field)]
    public class CustomFieldAttribute : Attribute
    {
        public Type type;
        public MethodInfo method = null;

        public CustomFieldAttribute(Type type)
        {
            this.type = type;
        }
        
        public CustomFieldAttribute(Type type, Action<byte[]> staticConstructor)
        {
            this.type = type;
            this.method = staticConstructor.Method;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class SizeFieldAttribute : Attribute
    {
        public SizeFieldAttribute(int size)
        {
        }
        
        public SizeFieldAttribute(FieldInfo field)
        {
        }
        
        public SizeFieldAttribute(string fieldstr)
        {
        }
    }
}