using OpenTK.Graphics.OpenGL4;
using System;

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
