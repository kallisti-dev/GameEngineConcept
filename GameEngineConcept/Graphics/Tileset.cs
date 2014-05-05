using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using OpenTK;

using GameEngineConcept.Graphics.Loaders;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public struct TileSet
    {
        public int TileWidth  { get; private set; }
        public int TileHeight { get; private set; }
        public Texture Texture 
        {
            get
            {
                if (t == null)
                    t = Texture.FromFile(fileName);
                return t;
            }
            private set
            {
                t = value;
            }
        }

        private string fileName;
        private Texture t;

        public TileSet(Texture tex, int tileWidth, int tileHeight) : this()
        {
            Debug.Assert(tex != null);
            Texture = tex;

        }

        public TileSet(string fileName, int tileWidth, int tileHeight) : this()
        {
            this.fileName = fileName;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public IEnumerable<Sprite> LoadTileMap(VertexBuffer buffer, Point[,] mapIndices, int depth)
        {

            SpriteLoader loader = new SpriteLoader(OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw, buffer);
            for (int i = 0; i < mapIndices.GetLength(0); ++i)
            {
                for (int j = 0; i < mapIndices.GetLength(1); ++j)
                {
                    Point tileCoord = mapIndices[i, j];
                    loader.Add(
                        Texture,
                        new Rectangle(tileCoord.X * TileWidth, tileCoord.Y * TileHeight, TileWidth, TileHeight),
                        new Vector2(i * TileWidth, j * TileHeight),
                        depth);
                }
            }
            return loader.LoadBuffer();
        }
    }
}
