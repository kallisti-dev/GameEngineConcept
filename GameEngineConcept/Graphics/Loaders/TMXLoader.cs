using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using NTiled;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics.Loaders
{

    public class TMXLoader
    {
        static IComparer<TiledTileset> tileSetComparer = Comparer<TiledTileset>.Create((x, y) => x.FirstId.CompareTo(y.Name));

        IBindableVertexBuffer buffer;
        string fileName;

        Lazy<TiledMap> lazyTileMap;
        Lazy<TiledTilesetCollection> lazyTileSets;
        Lazy<Dictionary<string, TiledLayer>> lazyLayers;
        Dictionary<int, Texture> textures;

        public TMXLoader(IBindableVertexBuffer buffer, string fileName)
        {
            textures = new Dictionary<int,Texture>();
            this.fileName = fileName;
            this.buffer = buffer;
            lazyTileMap = 
                new Lazy<TiledMap>(
                    () => (new TiledReader()).Read(fileName));
            lazyLayers = 
                new Lazy<Dictionary<string, TiledLayer>>(
                    () => lazyTileMap.Value.Layers.ToDictionary(ts => ts.Name, ts => ts));            
        }

        private TiledTileset findTileSet(int gId, out int localId)
        {
            var tileSet = lazyTileMap.Value.Tilesets.Reverse<TiledTileset>().First((ts) => ts.FirstId <= gId);
            localId = gId - tileSet.FirstId;
            return tileSet;
        }

        private Texture getTexture(TiledTileset ts)
        {
            Texture t;
            if(!textures.TryGetValue(ts.FirstId, out t))
                textures[ts.FirstId] = t = Texture.FromFile(ts.Image.Source);
            return t;
        }

        public IEnumerable<Sprite> LoadLayer(IBindableVertexBuffer buffer, string name, int depth = 0)
        {
            return LoadLayer(lazyLayers.Value[name], buffer, name, depth);
        }

        private IEnumerable<Sprite> LoadLayer(TiledLayer layer, IBindableVertexBuffer buffer, string name, int depth = 0)
        {
            TiledMap tileMap = lazyTileMap.Value;
            SpriteLoader loader = new SpriteLoader(BufferUsageHint.DynamicDraw, buffer);
            int mapX = 0, mapY = 0, 
                width = tileMap.TileWidth, 
                height = tileMap.TileHeight;
            foreach(var strValue in layer.ToString().Split(',')) 
            {
                int localId;
                var tileSet = findTileSet(Int32.Parse(strValue), out localId);
                int tileX = localId % tileMap.Height, 
                    tileY = localId / tileMap.Height;
                loader.Add(getTexture(tileSet), new Rectangle(tileX, tileY, width, height), new Vector2(mapX, mapY), depth);
                if(mapX >= layer.Width)
                {
                    mapX = 0;
                    mapY += height;
                }
                else
                    mapX += width;
            }
            return loader.LoadBuffer();
        }
    }
}
