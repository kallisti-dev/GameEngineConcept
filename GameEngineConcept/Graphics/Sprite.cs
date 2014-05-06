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

        public Vector2 Position
        {
            get { return startVertex.Value.position; }
            set 
            {
                var previous = startVertex.Value.position;
            }
        }

        Lazy<TexturedVertex2> startVertex;

        public Sprite(Texture tex, IBindableVertexBuffer buffer, int bufferInd, int depth = 0)
            : base(tex, PrimitiveType.Quads, new AttributedVertexBuffer(buffer, vAttributes), VertexIndices.Create(bufferInd, 4), null)
        {
            DrawDepth = depth;
            index = bufferInd;
            startVertex = new Lazy<TexturedVertex2>(() =>
                VBuffer.GetData<TexturedVertex2>(BufferIndex, 4)[0]
            );
        }
    }
}
