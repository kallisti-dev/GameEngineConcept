using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class Sprite : TexturedVertexSet, IDrawableDepth
    {

        private int index;
        public int DrawDepth { get; protected set; }
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
            DrawDepth = depth;
            index = bufferInd;

        }
    }
}
