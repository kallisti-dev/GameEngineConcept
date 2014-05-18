using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
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

        public TileScene(TileSet tileSet, Point[,] mapIndices, int depth = 0)
        {
            this.tileSet = tileSet;
            this.mapIndices = mapIndices;
            this.depth = depth;
            sprites = null;
        }

        public async override Task Load(ResourcePool pool, CancellationToken token)
        {
            buffer = await pool.GetBuffer();
            var loader = new SpriteLoader(BufferUsageHint.DynamicDraw, buffer);
            tileSet.LoadTileMap(loader, mapIndices, depth);
            sprites = loader.Load();
        }

        public override void Activate(GameState state)
        {
            state.AddDrawables(sprites);
        }
    }
}
