using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;

namespace GameEngineConcept.Graphics.Loaders
{
    using VertexBuffers;

    //vertex loader where output and state type are the same
    public abstract class BaseVertexLoader<BufferType, VIn, VOut> : BaseVertexLoader<BufferType, VIn, VOut, VOut>
        where BufferType : IVertexBuffer
        where VIn : struct
    {

        protected BaseVertexLoader(BufferUsageHint hint, BufferType buffer) : base(hint, buffer) { }

        protected sealed override IEnumerable<VOut> CreateVertexOutput(VOut state)
        {
            yield return state;
        }
    }

    //vertex loader with internal state type
    public abstract class BaseVertexLoader<BufferType, VIn, VState, VOut> 
        : ILoader<IEnumerable<VOut>>, IHasVertexBuffer<BufferType>
        where BufferType : IVertexBuffer
        where VIn : struct
    {

        public BufferType VBuffer {get; set;}
        protected int currentIndex;
        private BufferUsageHint hint;
        private DynamicArray<VIn> vList;
        private List<VState> stateList;


        protected BaseVertexLoader(BufferUsageHint hint, BufferType buffer)
        {
            this.VBuffer = buffer;
            vList = new DynamicArray<VIn>();
            stateList = new List<VState>();
            currentIndex = 0;
            this.hint = hint;
        }

        //Converts an internal state value into one or more output values
        //
        //all subclasses must define this conversion
        protected abstract IEnumerable<VOut> CreateVertexOutput(VState state);

        //queues vertices to be copied into the vertex buffer
        protected void AddVertices(VIn[] vertices)
        {
            for (int i = 0; i < vertices.Length; ++i)
            {
                vList.Add(vertices[i]);
            }
            currentIndex += vertices.Length;
        }

        //queues a new state value
        protected void AddState(VState s)
        {
            stateList.Add(s);
        }


        //dequeues all added state values and returns an enumeration of them
        protected IEnumerable<VOut> ConsumeAddedStates()
        {
            var outs = stateList.SelectMany(CreateVertexOutput);
            stateList.Clear();
            return outs;
        }

        //loads all queued vertices into the vertex buffer (should only be called once)
        protected void LoadBuffer()
        {
            VBuffer.LoadData(hint, vList.InternalArray);
        }

        //public-facing load method, which should load the buffer and produce an enumeration of output values
        //
        //subclasses may wish to override this to provide custom behavior
        public virtual IEnumerable<VOut> Load()
        {
            LoadBuffer();
            return ConsumeAddedStates();
        }
    }
}
