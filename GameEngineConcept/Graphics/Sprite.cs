using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class Sprite : TexturedVertexSet
    {
        public static VertexAttribute vAttributes;

        private int index;
        public int depth;
        public int BufferIndex
        {
            get { return index; }
            protected set
            {
                index = value;
                indices = new IndexRange(value, 4);
            }
        }

        public Sprite(Texture tex, IBindableVertexBuffer buffer, int bufferInd, int depth = 0, int[] enabledAttribs = null)
            : base(tex, PrimitiveType.Quads, new AttributedVertexBuffer(buffer, TexturedVertex2.vAttributes), new IndexRange(bufferInd, 4), enabledAttribs)
        {
            this.depth = depth;
            index = bufferInd;

        }
    }
}
