using OpenTK.Graphics.OpenGL4;
using System;

namespace GameEngineConcept.Graphics.VertexAttributes
{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
    public class VertexAttribute : Attribute
    {
        public int index, nComponents, offset, stride;
        public VertexAttribPointerType type;
        public bool normalized;

        public VertexAttribute() { }

        public VertexAttribute(int index, int nComponents, VertexAttribPointerType type, bool normalized = true, int offset = 0, int stride = 0)
        {
            this.index = index;
            this.nComponents = nComponents;
            this.type = type;
            this.normalized = normalized;
            this.offset = offset;
            this.stride = stride;
        }

        public void Load()
        {
            GL.VertexAttribPointer(index, nComponents, type, normalized, stride, offset);
        }

        public void Enable()
        {
            GL.EnableVertexAttribArray(index);
        }

        public void Disable()
        {
            GL.DisableVertexAttribArray(index);
        }
    }
}
