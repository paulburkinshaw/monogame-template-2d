using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Template.TwoD.Core;
using MonoSprite;

namespace MonoGame.Template.TwoD.Rendering;

public interface IGameRenderer
{
    public void Render(GameTime gameTime);

}

public class GameRenderer : IGameRenderer
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;
    private readonly IGameSettings _gameSettings;

    private RenderTarget2D _entitiesRenderTarget;
    private RenderTarget2D _tilemapRenderTarget;

    public GameRenderer(
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        ISpriteService spriteService,
        IGameSettings gameSettings
    )
    {
        _gameSettings = gameSettings;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;

        _entitiesRenderTarget = new RenderTarget2D(
            graphicsDevice,
            _gameSettings.InternalSize.Width,
            _gameSettings.InternalSize.Height
        );


    }

    public void Render(GameTime gameTime)
    {
        _graphicsDevice.SetRenderTarget(_entitiesRenderTarget);
        _graphicsDevice.Clear(Color.Transparent);
        _spriteBatch.Begin();

        DrawEntities();

        _spriteBatch.End();
        _graphicsDevice.SetRenderTarget(null);
    }

    private void DrawEntitiesToOffScreenRenderTarget() { }
    private void DrawTilemap() { }
    private void DrawEntities()
    {
        
    }
}



