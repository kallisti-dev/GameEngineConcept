using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    //allows recycling of allocated buffer objects. Use Request() to request an unused buffer object.
    //All Request() calls should have a corresponding Release() call for when the buffer is no longer needed, so that other consumers can reuse the buffer.
    public class VertexBufferPool
    {
        const uint defaultInitialSize = 20;
        const int defaultMaxSize = 100;

        static Func<uint, uint> defaultResizer = (n) => n == 0 ? defaultInitialSize : (2 * n);

        public uint buffersAllocated;
        public uint maxAllocated;
        readonly LinkedList<VertexBuffer> bufferPool;
        readonly Func<uint, uint> resizer;
        readonly Queue<TaskCompletionSource<VertexBuffer>> waiters;

        //Constructor
        //    initialSize: starting size of the pool
        //    maxSize:     maximum number of buffers that can be allocated from this pool
        //    resizer:     callback used to determine new pool size when a resize is required.
        public VertexBufferPool(
            uint initialSize = defaultInitialSize, 
            uint maxSize = defaultMaxSize, 
            Func<uint,uint> resizer = null)
        {
            Debug.Assert(maxSize >= initialSize);
            waiters = new Queue<TaskCompletionSource<VertexBuffer>>();
            bufferPool = new LinkedList<VertexBuffer>();
            foreach (VertexBuffer b in VertexBuffer.Allocate(initialSize))
            {
                bufferPool.AddFirst(b);
            }
            this.buffersAllocated = initialSize;
            this.maxAllocated = maxSize;
            this.resizer = resizer ?? defaultResizer;
        }


        //Requests a buffer, passes it to the given callback, and automatically releases buffer after callback completes or callback triggers an exception.
        public async Task WithBuffer(Action<VertexBuffer> callback)
        {
            var b = await Request();
            try
            {
                callback(b);
            }
            finally
            {
                Release(b);
            }
        }

        //an asynchronous request for a VertexBuffer from the pool. All calls should have a corresponding Release() call
        //for when the buffer is no longer needed, so that other consumers can reuse the buffer.
        public Task<VertexBuffer> Request()
        {
            lock (bufferPool)
            {
                if (bufferPool.Count > 0)
                {
                    var buf = GetBuffer();
                    if (buf == null)
                        return AddWaiter();
                    else
                        return Task.FromResult(buf);
                }
                else
                {
                    return AddWaiter();
                }
            }
        }

        //returns a buffer to the pool so that others may use it
        public void Release(VertexBuffer b)
        {
            TaskCompletionSource<VertexBuffer> waiter = null;
            lock (bufferPool)
            {
                if (waiters.Count > 0)
                    waiter = waiters.Dequeue();
                else
                    bufferPool.AddLast(b);
            }
            if(waiter != null) waiter.TrySetResult(b);
        }


        private VertexBuffer GetBuffer()
        {
            if (bufferPool.Count == 0)
            {
                if (!Resize()) return null;
            }
            var b = bufferPool.Last.Value;
            bufferPool.RemoveLast();
            return b;
        }

        private Task<VertexBuffer> AddWaiter()
        {
            var source = new TaskCompletionSource<VertexBuffer>();
            waiters.Enqueue(source);
            return source.Task;
        }

        //resize buffer pool. returns false if no resize occurs because pool is already at max capacity;
        private bool Resize()
        {
            if (buffersAllocated == maxAllocated)
                return false;
            uint new_size = Math.Min(maxAllocated, resizer(buffersAllocated));
            Debug.Assert(new_size > buffersAllocated);
            foreach (var b in VertexBuffer.Allocate(new_size - buffersAllocated))
            {
                bufferPool.AddFirst(b);
            }
            buffersAllocated = new_size;
            return true;
        }
    }
}
