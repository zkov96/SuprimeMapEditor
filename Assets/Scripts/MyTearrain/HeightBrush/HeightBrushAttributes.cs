using System;

namespace MyTearrain.HeightBrush
{
    public class HeightBrushAttribute:Attribute
    {
        public readonly HeightBrushType type;
        public readonly Type settings;
        
        public HeightBrushAttribute(HeightBrushType type = HeightBrushType.Static, Type settings=null)
        {
            this.type = type;
            this.settings = settings;
        }
        
        public enum HeightBrushType
        {
            Static,
            Dynamic
        }
    }

    public class StaticHeightFuncAttribute : Attribute
    {
    }
}