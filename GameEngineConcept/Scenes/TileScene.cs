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
    class TileScene : IScene
    {
        VertexBuffer buffer;
        SpriteLoader loader;
        string tilemapFile;
        int tileWidth;
        int tileHeight;
        Point[,] mapIndices;
        int depth;
        IEnumerable<Sprite> sprites;
        TaskCompletionSource<bool> source;

        public bool IsLoaded { get { return sprites != null; } }

        public TileScene(VertexBuffer buffer, string tilemapFile, int tileWidth, int tileHeight, Point[,] mapIndices, int depth = 0)
        {
            this.buffer = buffer;
            this.tilemapFile = tilemapFile;
            this.tileHeight = tileHeight;
            this.tileWidth = tileWidth;
            this.mapIndices = mapIndices;
            this.depth = depth;
            sprites = null;
            source = new TaskCompletionSource<bool>();
        }

        public void Load()
        {
            if (IsLoaded) return;
            loader = new SpriteLoader(OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw, buffer);
            Texture tileMap = Texture.FromBitmap(tilemapFile);
            for(int i = 0; i < mapIndices.GetLength(0); ++i)
            {
                for(int j = 0; i < mapIndices.GetLength(1); ++j)
                {
                    Point tileCoord = mapIndices[i,j];
                    loader.Add(
                        tileMap,
                        new Rectangle(tileCoord.X * tileWidth, tileCoord.Y * tileHeight, tileWidth, tileHeight),
                        new Vector2(i * tileWidth, j * tileHeight),
                        depth);
                }
            }
            sprites = loader.LoadBuffer();
        }

        public void Unload()
        {
            sprites = null;
        }

        public void Activate(EngineWindow window)
        {
            Debug.Assert(IsLoaded);
            window.AddDrawables(sprites);
        }

        public void Deactivate(EngineWindow window)
        {
            if (!IsLoaded) return;
            window.RemoveDrawables(sprites);
        }



    }
}
