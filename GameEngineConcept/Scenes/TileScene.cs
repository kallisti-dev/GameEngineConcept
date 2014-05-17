using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace GameEngineConcept.Scenes
{
    using Graphics;
    using Graphics.Loaders;
    using Graphics.VertexBuffers;

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
            var loader = new SpriteLoader(BufferUsageHint.DynamicDraw, buffer);
            tileSet.LoadTileMap(loader, mapIndices, depth);
            sprites = loader.Load();
        }

        public override void Unload(Pool<VertexBuffer> vPool)
        {
            vPool.Release(buffer);
            sprites = null;
        }

        public override void Activate(GameState state)
        {
            Debug.Assert(IsLoaded);
            state.AddDrawables(sprites);
        }



    }
}
