using GameEngineConcept.Graphics.Loaders;
using OpenTK;
using System;
using System.Diagnostics;
using System.Drawing;

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

        public TileSet(Texture t, string fileName, int tileWidth, int tileHeight)
        {
            this.fileName = fileName;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            this.t = new Lazy<Texture>(() => { t.LoadImageFile(fileName); return t; });

        }

        public void LoadTileMap(SpriteLoader loader, Point[,] mapIndices, int depth)
        {

            for (int i = 0; i < mapIndices.GetLength(0); ++i)
            {
                for (int j = 0; i < mapIndices.GetLength(1); ++j)
                {
                    Point tileCoord = mapIndices[i, j];
                    loader.AddSprite(
                        Texture,
                        new Vector2(i * TileWidth, j * TileHeight),
                        new Rectangle(tileCoord.X * TileWidth, tileCoord.Y * TileHeight, TileWidth, TileHeight),
                        depth);
                }
            }
        }
    }
}
