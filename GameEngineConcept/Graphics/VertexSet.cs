using System;
using OpenTK.Graphics.OpenGL4;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class VertexSet : IDrawable, IHasVertexAttributes, IHasVertexBuffer<IAttributedVertexBuffer>
    {
        public IAttributedVertexBuffer VBuffer { get; protected set; }
        public PrimitiveType DrawMode { get; protected set; }
        protected VertexIndices indices;
        private int[] enabledAttribs;
        public int[] EnabledAttributes { 
            get 
            {
                if(enabledAttribs == null) 
                    enabledAttribs = VBuffer.GetAttributeIndices();
                return enabledAttribs;
            }
            set
            {
                int[] allIndices = VBuffer.GetAttributeIndices();
                foreach(int i in value) 
                {
                    if(-1 == Array.IndexOf(allIndices, i)) {
                        throw new IndexOutOfRangeException(i + " is not a valid attribute index. Valids indices are: " + allIndices);
                    }                   
                }           
                enabledAttribs = value;
            }
        }


        public VertexAttribute[] VertexAttributes { get { return VBuffer.VertexAttributes; } }

        protected VertexSet() { }

        public VertexSet(PrimitiveType mode, IAttributedVertexBuffer buffer, VertexIndices indices, int[] enabledAttribs = null)
        {
            DrawMode = mode;
            VBuffer = buffer;
            EnabledAttributes = enabledAttribs;
            this.indices = indices;
        }

        public int CompareTo(IDrawable d)
        {
            return -d.CompareTo(VBuffer);
        }

        public int CompareTo(IVertexBuffer b)
        {
            return VBuffer.CompareTo(b);
        }
         
        public virtual void Draw()
        {
            VBuffer.Bind(BufferTarget.ArrayBuffer, EnabledAttributes, (boundBuffer) =>
            {
                boundBuffer.Draw(DrawMode, indices);
            });
        }
    }
}
