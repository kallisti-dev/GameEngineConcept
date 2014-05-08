using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using NTiled;

namespace GameEngineConcept.Graphics.Loaders
{
    using Graphics.VertexBuffers;

    public class TMXLoader : SpriteLoader
    {
        Lazy<TiledMap> _lazyTileMap;
        Lazy<Dictionary<string, TiledLayer>> _lazyLayers;

        Dictionary<int, Texture> textures;
        Pool<Texture> texPool;

        TiledMap TileMap { get { return _lazyTileMap.Value;  } }
        Dictionary<string, TiledLayer> TiledLayers { get { return _lazyLayers.Value; } }


        public Dictionary<string, IEnumerable<Sprite>> Layers {get; private set;}

        public TMXLoader(IBindableVertexBuffer buffer, Pool<Texture> texPool, string fileName)
            : base(BufferUsageHint.DynamicDraw, buffer)
        {
            textures = new Dictionary<int,Texture>();
            this.texPool = texPool;
           _lazyTileMap = 
                new Lazy<TiledMap>(
                    () => (new TiledReader()).Read(fileName));
            _lazyLayers = 
                new Lazy<Dictionary<string, TiledLayer>>(
                    () => TileMap.Layers.ToDictionary(ts => ts.Name, ts => ts));            
        }

        public Task AddAllLayers(int depth = 0)
        {
            return AddAllLayers((layer) => depth);
        }

        public async Task AddAllLayers(Func<TiledLayer, int> depthFunction)
        {
            foreach(var layer in TileMap.Layers)
            {
                if (!Layers.ContainsKey(layer.Name))
                    await AddLayer(layer.Name, depthFunction(layer));
            }
        }

        public Task AddLayer(string name, int depth = 0)
        {
            return _AddLayer(TiledLayers[name], name, depth);
        }

        public override IEnumerable<Sprite> Load()
        {
            LoadBuffer();
            return Layers.Values.Join().Concat(ConsumeAddedStates());
        }

        private async Task<IEnumerable<Sprite>> _AddLayer(TiledLayer layer, string name, int depth = 0)
        {
            TiledMap tileMap = TileMap;
            int mapX = 0, mapY = 0,
                width = tileMap.TileWidth,
                height = tileMap.TileHeight;
            foreach (var strValue in layer.ToString().Split(','))
            {
                int localId;
                var tileSet = findTileSet(Int32.Parse(strValue), out localId);
                int tileX = localId % tileMap.Height,
                    tileY = localId / tileMap.Height;
                AddSprite(await getTexture(tileSet), new Vector2(mapX, mapY), new Rectangle(tileX, tileY, width, height), depth);
                if (mapX >= layer.Width)
                {
                    mapX = 0;
                    mapY += height;
                }
                else
                    mapX += width;
            }
            return Layers[name] = ConsumeAddedStates();
        }

        private TiledTileset findTileSet(int gId, out int localId)
        {
            var tileSet = TileMap.Tilesets.Reverse<TiledTileset>().First((ts) => ts.FirstId <= gId);
            localId = gId - tileSet.FirstId;
            return tileSet;
        }

        private async Task<Texture> getTexture(TiledTileset ts)
        {
            Texture t;
            if (!textures.TryGetValue(ts.FirstId, out t))
            {
                t = await texPool.Request();
                t.LoadImageFile(ts.Image.Source);
                textures[ts.FirstId] = t;
            }
            return t;
        }
    }
}
