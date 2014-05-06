using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    //convenience functions for resource pools
    public static class Pool
    {
        public static Pool<VertexBuffer> CreateBufferPool()
        {
            return new Pool<VertexBuffer>(VertexBuffer.Allocate);
        }
        public static Pool<Texture> CreateTexturePool()
        {
            return new Pool<Texture>(Texture.Allocate);
        }
    }

    //allows recycling of allocated openGL resources. Use Request() to request a resource.
    //All Request() calls should have a corresponding Release() call for when the buffer is no longer needed, so that other consumers can reuse the buffer.
    public class Pool<T>
    {
        const uint defaultInitialSize = 20;
        const int defaultMaxSize = 100;
        static Func<uint, uint> defaultResizer = (n) => n == 0 ? defaultInitialSize : (2 * n);

        public uint nAllocated;
        public uint maxAllocable;
        readonly LinkedList<T> pool;
        readonly Func<uint, uint> resizer;
        readonly Queue<TaskCompletionSource<T>> waiters;
        readonly Func<uint, IEnumerable<T>> nAllocator;

        //Constructor
        //    nAllocator:  allocation function for type T. Takes number of T's to allocate as its parameter
        //                 and returns an IEnumerable of the allocated objects
        //    initialSize: starting size of the pool
        //    maxSize:     maximum number of resources that can be allocated from this pool
        //    resizer:     callback used to determine new pool size when a resize is required.
        public Pool(
            Func<uint, IEnumerable<T>> nAllocator,
            uint initialSize = defaultInitialSize,
            uint maxSize = defaultMaxSize,
            Func<uint, uint> resizer = null)
        {
            Debug.Assert(maxSize >= initialSize);
            waiters = new Queue<TaskCompletionSource<T>>();
            pool = new LinkedList<T>();
            foreach (T o in nAllocator(initialSize))
            {
                pool.AddFirst(o);
            }
            this.nAllocated = initialSize;
            this.maxAllocable = maxSize;
            this.resizer = resizer ?? defaultResizer;
        }


        //Requests a resource, passes it to the given callback, and automatically releases it after callback completes or callback triggers an exception.
        public async Task With(Action<T> callback)
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
        public Task<T> Request()
        {
            lock (pool)
            {
                if (pool.Count > 0)
                {
                    var r = GetOne();
                    if (r.Equals(default(T)))
                        return Task.FromResult(r);
                }
                var source = new TaskCompletionSource<T>();
                waiters.Enqueue(source);
                return source.Task;
            }
        }

        //returns a buffer to the pool so that others may use it
        public void Release(T resource)
        {
            TaskCompletionSource<T> waiter = null;
            lock (pool)
            {
                if (waiters.Count > 0)
                    waiter = waiters.Dequeue();
                else
                    pool.AddLast(resource);
            }
            if (waiter != null) waiter.TrySetResult(resource);
        }


        private T GetOne()
        {
            if (pool.Count == 0)
            {
                if (!Resize()) return default(T);
            }
            var b = pool.Last.Value;
            pool.RemoveLast();
            return b;
        }

        //resize buffer pool. returns false if no resize occurs because pool is already at max capacity;
        private bool Resize()
        {
            if (nAllocated == maxAllocable)
                return false;
            uint new_size = Math.Min(maxAllocable, resizer(nAllocated));
            Debug.Assert(new_size > nAllocated);
            foreach (var b in nAllocator(new_size - nAllocated))
            {
                pool.AddFirst(b);
            }
            nAllocated = new_size;
            return true;
        }
    }
}
