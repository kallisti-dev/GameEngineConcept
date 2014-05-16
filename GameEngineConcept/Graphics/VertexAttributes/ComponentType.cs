using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace GameEngineConcept.Graphics.VertexAttributes
{
    using ExtensionMethods;

    public sealed class ComponentType : AttributeDescriptor
    {
        VertexAttribPointerType t;

        public ComponentType(VertexAttribPointerType type)
        {
            t = type;
        }

        public ComponentType(Type type)
        {
            var aType = type.GetVertexAttribPointerType();
            if (aType.HasValue)
                t = aType.Value;
            else
                throw new ArgumentException("Invalid component type " + type);
        }

        public override void AddFields(VertexAttribute a)
        {
            a.type = t;
        }
    }
}
