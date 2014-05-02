using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept
{
    public struct VertexAttribute
    {
        public int index, nComponents, offset, stride;
        public VertexAttribPointerType type;
        public bool normalized;

        public VertexAttribute(int index, int nComponents, VertexAttribPointerType type, bool normalized, int stride = 0, int offset = 0)
        {
            this.index = index;
            this.nComponents = nComponents;
            this.type = type;
            this.normalized = normalized;
            this.offset = offset;
            this.stride = stride;
        }
    }
}
