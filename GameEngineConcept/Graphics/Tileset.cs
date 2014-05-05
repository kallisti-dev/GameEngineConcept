using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using OpenTK;

using GameEngineConcept.Graphics.Loaders;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class TileSet
    {
        public int TileWidth  { get; private set; }
        public int TileHeight { get; private set; }

        //lazily loads texture from file if given filename instead of texture
        public Texture Texture 
        {
            get { return t.Value; }
        }

        private string fileName;
        private Lazy<Texture> t;

        public TileSet(Texture tex, int tileWidth, int tileHeight)
        {
            Debug.Assert(tex != null);
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            t = new Lazy<Texture>(() => tex);
        }

        public TileSet(string fileName, int tileWidth, int tileHeight)
        {
            this.fileName = fileName;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            t = new Lazy<Texture>(() => Texture.FromFile(fileName));

        }

        public IEnumerable<Sprite> LoadTileMap(IBindableVertexBuffer buffer, Point[,] mapIndices, int depth)
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
