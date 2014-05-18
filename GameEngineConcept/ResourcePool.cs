using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameEngineConcept
{
    using Graphics;
    using Graphics.VertexBuffers;
    using Util;
    public class ResourcePool
    {
        Pool<Texture> tPool;
        Pool<VertexBuffer> vPool;

        List<Texture> tList = new List<Texture>();
        List<VertexBuffer> vList = new List<VertexBuffer>();

        public event Action OnRelease;

        internal ResourcePool(Pool<Texture> tPool, Pool<VertexBuffer> vPool)
        {
            this.tPool = tPool;
            this.vPool = vPool;
        }

        //Allocates a texture. The texture is deallocated when Unload method is called
        public Task<Texture> GetTexture() 
        {
            return _Get(tPool, tList);
        }

        //Allocates a VBO. The VBO is deallocated when Unload is called
        public Task<VertexBuffer> GetBuffer()
        {
            return _Get(vPool, vList);
        }

        internal void Release()
        {
            _Release(vPool, vList);
            _Release(tPool, tList);
            OnRelease();
        }

        //creates a new ResourcePool that refers to the same internal resources,
        //but manages a new allocation set
        internal ResourcePool Copy()
        {
            return new ResourcePool(tPool, vPool);
        }

        private async Task<T> _Get<T>(Pool<T> pool, List<T> list)
        {
            var resource = await pool.Request();
            list.Add(resource);
            return resource;
        }

        private void _Release<T>(Pool<T> pool, List<T> list)
        {
            foreach (var x in list) {
                pool.Release(x);
            }
        }
    }
}
