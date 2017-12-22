using System;

namespace MyTearrain.BrushMasks
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MaskAttribute:Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class StaticMaskFunkAttribute:Attribute
    {
    }
}