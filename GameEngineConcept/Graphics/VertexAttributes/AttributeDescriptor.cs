using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL4;

namespace GameEngineConcept.Graphics.VertexAttributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited=true)]
    public abstract class AttributeDescriptor : Attribute
    {
        public abstract void AddFields(VertexAttribute a);
    }
}
