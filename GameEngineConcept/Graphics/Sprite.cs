using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class Sprite : TexturedVertexSet, IDrawableDepth
    {
        static VertexAttribute[] vAttributes = TexturedVertex2.vAttributes;

        private int index;
        public int DrawDepth { get; protected set; }
        public int BufferIndex
        {
            get { return index; }
            protected set
            {
                index = value;
                indices =  VertexIndices.Create(value, 4);
            }
        }

        TexturedVertex2 StartVertex
        {
            get
            {
                if (startVertex.HasValue)
                    return startVertex.Value;
                var v = VBuffer.GetData<TexturedVertex2>(BufferIndex, 4)[0];
                startVertex = v;
                return v;
            }
            set
            {
                startVertex = value;
            }
        }

        TexturedVertex2? startVertex = null;

        public Sprite(Texture tex, IBindableVertexBuffer buffer, int bufferInd, int depth = 0)
            : base(tex, PrimitiveType.Quads, new AttributedVertexBuffer(buffer, vAttributes), VertexIndices.Create(bufferInd, 4), null)
        {
            DrawDepth = depth;
            index = bufferInd;
        }
    }
}
