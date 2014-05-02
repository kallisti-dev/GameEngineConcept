using System;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Buffers;

namespace GameEngineConcept
{
    public class TexturedVertices : IHasVertexBufferIndices<IBindableVertexBuffer>
    {
        public Texture Texture { get; protected set; }
        public IBindableVertexBuffer Buffer { get; protected set; }
        public uint[] BufferIndices { get; protected set; }
        public PrimitiveType DrawMode { get; protected set; }

        //TODO property: bool isLoaded

        protected TexturedVertices() { }

        public TexturedVertices(PrimitiveType mode, Texture tex, IBindableVertexBuffer buffer, uint[] indices, int depth = 0)
        {
            DrawMode = mode;
            Texture = Texture;
            Buffer = buffer;
            BufferIndices = indices;
        }
    }
}
