using System;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Buffers;

namespace GameEngineConcept
{
    public class TexturedVertices : IHasVertexBufferIndices<IVertexBufferBindable>
    {
        public Texture Texture { get; protected set; }
        public IVertexBufferBindable Buffer { get; protected set; }
        public uint[] BufferIndices { get; protected set; }
        public PrimitiveType DrawMode { get; protected set; }

        //TODO property: bool isLoaded

        protected TexturedVertices() { }

        public TexturedVertices(PrimitiveType mode, Texture tex, IVertexBufferBindable buffer, uint[] indices, int depth = 0)
        {
            DrawMode = mode;
            Texture = Texture;
            Buffer = buffer;
            BufferIndices = indices;
        }

    }
}
