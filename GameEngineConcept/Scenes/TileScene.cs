using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.Loaders;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Scenes
{
    class TileScene : Scene
    {
        VertexBuffer buffer;
        TileSet tileSet;
        Point[,] mapIndices;
        int depth;
        IEnumerable<Sprite> sprites;

        public override bool IsLoaded { get { return sprites != null; } }

        public TileScene(TileSet tileSet, Point[,] mapIndices, int depth = 0)
        {
            this.tileSet = tileSet;
            this.mapIndices = mapIndices;
            this.depth = depth;
            sprites = null;
        }

        public async override Task Load(Pool<VertexBuffer> vPool)
        {
            buffer = await vPool.Request();
            sprites = tileSet.LoadTileMap(buffer, mapIndices, depth);
        }

        public override void Unload(Pool<VertexBuffer> vPool)
        {
            vPool.Release(buffer);
            sprites = null;
        }

        public override void Activate(EngineWindow window)
        {
            Debug.Assert(IsLoaded);
            window.AddDrawables(sprites);
        }

        public override void Deactivate(EngineWindow window)
        {
            if (!IsLoaded) return;
            window.RemoveDrawables(sprites);
        }



    }
}
