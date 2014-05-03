using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class Sprite : TexturedVertexSet
    {

        private uint index;
        public uint VertexBufferStartIndex {
            get { return index; }
            protected set 
            { 
                index = value;
                uint[] indices = new uint[4];
                for(uint i = 0; i < 4; ++i) indices[i] = value+i;
                VertexBufferIndices = indices;
            }
        }
        public int Depth { get; set; }

        public Sprite(Texture tex, IAttributedVertexBuffer buffer, uint bufferInd, int depth = 0) : base()
        {
            Texture = tex;
            VertexBuffer = buffer;
            VertexBufferStartIndex = bufferInd;
            Depth = depth;
            DrawMode = PrimitiveType.Quads;
        }
    }
}
