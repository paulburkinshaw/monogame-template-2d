using TiledDotNet;
using TiledDotNet.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace MonoTiled;

public interface ITilemapService
{
    Tilemap Load(string tilemapFilepath, string tilesetFilepath, Rectangle internalSize, Rectangle windowSize);
}

public class TilemapService : ITilemapService
{
    private readonly ITiledTilemapService _tiledTilemapService;
    private readonly ITilesetTextureService _tilesetTextureService;
    private readonly ContentManager _content;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;

    public TilemapService(
       ITiledTilemapService tiledTilemapService,
       ITilesetTextureService tilesetTextureService,
       ContentManager content,
       GraphicsDevice graphicsDevice,
       SpriteBatch spriteBatch)
    {
        _tiledTilemapService = tiledTilemapService;
        _tilesetTextureService = tilesetTextureService;
        _content = content;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
    }

    public Tilemap Load(string tilemapFilepath, string tilesetFilepath, Rectangle internalSize, Rectangle windowSize)
    {
        // Get Tiled tilemap from file
        var tiledTilemap = _tiledTilemapService.GetTiledTilemap(tilemapFilepath);
        var tileLayer = tiledTilemap.Layers.Where(x => x.TiledLayerType == TiledLayerType.TileLayer).FirstOrDefault() as TileLayer;
        var tiledTiles = tileLayer.Tiles;

        // Load tileset textures into tile service tilset texture dictionary keyed by the tileset's firstGID
        // This allows us to easily find the correct tileset texture for a given tilesetId
        foreach (var tileset in tiledTilemap.Tilesets)
        {
            // Load tileset texture from file
            // Assumes tileset image files are in a folder named "Tilesets" in the Content project and are set to "Do not copy" in their properties
            var _tilesetTexture = _content.Load<Texture2D>($"{tilesetFilepath}/{tileset.Name}");
            _tilesetTextureService.AddTilesetTexture(tileset.FirstGID, _tilesetTexture);
        }

        // Convert Tiled tiles to MonoGame tiles
        var tiles = new Tile[tileLayer.TileCountY, tileLayer.TileCountX];

        for (int y = 0; y < tiledTilemap.TileCountY; y++)
        {
            for (int x = 0; x < tiledTilemap.TileCountX; x++)
            {
                var tiledTile = tiledTiles[y, x];
                if (tiledTile == null)
                    continue;

                tiles[y, x] = new Tile(
                    tileSetId: tiledTile.TileSetId,
                    localTileId: tiledTile.LocalTileId,
                    tileSourceRectangle: new Rectangle(tiledTile.TileSourceRectangle.X,
                                                        tiledTile.TileSourceRectangle.Y,
                                                        tiledTile.TileSourceRectangle.Width,
                                                        tiledTile.TileSourceRectangle.Height),
                    positionX: x * tiledTile.TileSourceRectangle.Width,
                    positionY: y * tiledTile.TileSourceRectangle.Height,
                    tilesetTextureService: _tilesetTextureService
                );

            }
        }

        var tilemapRenderer = new TilemapRenderer(
            _graphicsDevice,
            _spriteBatch,
            internalSize,
            windowSize,
            _tilesetTextureService,
            tiledTilemap.TileCountX,
            tiledTilemap.TileCountY,
            tiles
        );

        var mapTitle = tiledTilemap.Properties.FirstOrDefault(x => x.Name.Equals("MapTitle"))?.Value as string;

        var tilemap = new Tilemap(
            tiledTilemap.TileCountX,
            tiledTilemap.TileCountY,
            tiledTilemap.TileWidth,
            tiledTilemap.TileHeight,
            mapTitle,
            tiles,
            tilemapRenderer
        );

        // tilemap.TilemapRenderer = tilemapRenderer;

        return tilemap;
    }
}
