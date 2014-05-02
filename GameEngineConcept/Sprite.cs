using System;
using System.Collections.Generic;
using System.Linq;

using GameEngineConcept.Buffers;

namespace GameEngineConcept
{
    public class Sprite : TexturedVertices
    {

        private int index;
        public int BufferIndex {
            get { return index; }
            set 
            { 
                index = value;
                BufferIndices = Enumerable.Range(value, 4).Cast<uint>().ToArray();
            }
        }
        public int Depth { get; set; }

        public Sprite(Texture tex, VertexBuffer buffer, int bufferInd, int depth = 0) : base()
        {
            Texture = tex;
            Buffer = buffer;
            BufferIndex = bufferInd;
            Depth = depth;
        }
    }
}
