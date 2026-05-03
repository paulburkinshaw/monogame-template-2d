using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTiled;
public interface ITilemapRenderer
{
    void Draw();
}

/// <summary>
/// TODO: Support static tilemaps
/// Currently this class is designed for dynamic tilemaps where the tiles can change each frame, so it renders each tile every frame. 
/// For static tilemaps (i.e. tilemaps that do not change) it would be more efficient to render the tilemap to a texture once and then draw that texture each frame.
/// </summary>
public class TilemapRenderer : ITilemapRenderer
{
    private readonly SpriteBatch _spriteBatch;
    private readonly ITilesetTextureService _tilesetTextureService;

    private int _tileCountX;
    private int _tileCountY;
    private Tile[,] _tiles;

    public TilemapRenderer(
        SpriteBatch spriteBatch,
        ITilesetTextureService tilesetTextureService,
        int tileCountX,
        int tileCountY,
        Tile[,] tiles)
    {
        _spriteBatch = spriteBatch;
        _tilesetTextureService = tilesetTextureService;
        _tileCountX = tileCountX;
        _tileCountY = tileCountY;
        _tiles = tiles;
    }

    public void Draw()
    {
        for (int y = 0; y < _tileCountY; y++)
        {
            for (int x = 0; x < _tileCountX; x++)
            {
                var tile = _tiles[y, x];

                if (tile == null) continue;

                _spriteBatch.Draw(_tilesetTextureService.GetTilesetTexture(tile.TileSetId),
                    new Vector2(tile.PositionX, tile.PositionY),
                    new Rectangle(tile.TileSourceRectangle.X, tile.TileSourceRectangle.Y, tile.TileSourceRectangle.Width, tile.TileSourceRectangle.Height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
        }   
    }
}

