using GameEngineConcept.Graphics.VertexBuffers;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

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

        public int DrawDepth { get; protected set; }

        protected VertexSet() { }

        public VertexSet(PrimitiveType mode, IAttributedVertexBuffer buffer, VertexIndices indices, ISet<int> enabledAttribs = null, int depth = 0)
        {
            DrawMode = mode;
            VBuffer = buffer;
            EnabledAttributes = enabledAttribs;
            this.indices = indices;
            DrawDepth = depth;
        }

        public int CompareTo(IDrawable d)
        {
            return DrawDepth.CompareTo(d.DrawDepth);
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
