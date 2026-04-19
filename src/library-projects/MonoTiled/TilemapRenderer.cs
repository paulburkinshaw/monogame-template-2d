using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTiled;
public interface ITilemapRenderer
{
    void Draw();
}

public class TilemapRenderer : ITilemapRenderer
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;

    private readonly ITilesetTextureService _tilesetTextureService;

    private static Rectangle _internalSize;
    private static Rectangle _windowSize;

    private int _tileCountX;
    private int _tileCountY;
    private Tile[,] _tiles;

    public TilemapRenderer(
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        Rectangle internalSize,
        Rectangle windowSize,
        ITilesetTextureService tilesetTextureService,
        int tileCountX,
        int tileCountY,
        Tile[,] tiles)
    {
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _tilesetTextureService = tilesetTextureService;
        _internalSize = internalSize;
        _windowSize = windowSize;
        _tileCountX = tileCountX;
        _tileCountY = tileCountY;
        _tiles = tiles;

        DrawTilesToRenderTarget();
    }

    // Draw all tiles to a single render target for optimized rendering
    // Assumes the tilemap is static and does not change during gameplay
    private void DrawTilesToRenderTarget()
    {
        _renderTarget = new RenderTarget2D(_graphicsDevice, _internalSize.Width, _internalSize.Height);

        _graphicsDevice.SetRenderTarget(_renderTarget);
        // _graphicsDevice.Clear(Color.White);
        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

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

        _spriteBatch.End();
        _graphicsDevice.SetRenderTarget(null);
    }

    public void Draw()
    {
        _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), Color.White);
    }

}

