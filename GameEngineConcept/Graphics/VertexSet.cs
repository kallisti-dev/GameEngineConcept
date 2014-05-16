using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    using VertexAttributes;

    public class VertexSet : IDrawable, IHasVertexAttributes, IHasVertexBuffer<IAttributedVertexBuffer>
    {
        public IAttributedVertexBuffer VBuffer { get; protected set; }
        public PrimitiveType DrawMode { get; protected set; }
        protected VertexIndices indices;
        private ISet<int> enabledAttribs;
        public ISet<int> EnabledAttributes { 
            get 
            {
                if(enabledAttribs == null) 
                    enabledAttribs = VBuffer.VertexAttributes.Indices;
                return enabledAttribs;
            }
            set
            {
                ISet<int> allIndices = VBuffer.VertexAttributes.Indices;
                foreach(int i in value) 
                {
                    if(!allIndices.Contains(i)) {
                        throw new IndexOutOfRangeException(i + " is not a valid attribute index. Valids indices are: " + allIndices);
                    }               
                }           
                enabledAttribs = value;
            }
        }


        public VertexAttributeSet VertexAttributes { get { return VBuffer.VertexAttributes; } }

        protected VertexSet() { }

        public VertexSet(PrimitiveType mode, IAttributedVertexBuffer buffer, VertexIndices indices, ISet<int> enabledAttribs = null)
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
