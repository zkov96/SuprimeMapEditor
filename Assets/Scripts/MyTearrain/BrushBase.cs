using System;
using System.Collections.Generic;
using System.Linq;
using MyTearrain.HeightBrush;

namespace MyTearrain
{
    public class BrushBase
    {
        private List<Type> staticBrushes;
        
        public BrushBase()
        {
            staticBrushes=this.GetType().Assembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(HeightBrushAttribute))).ToList();
        }
    }
}