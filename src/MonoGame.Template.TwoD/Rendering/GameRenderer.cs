using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Template.TwoD.Core;
using MonoGame.Template.TwoD.Gameplay.GameEntities;
using MonoSprite;
using MonoTiled;

namespace MonoGame.Template.TwoD.Rendering;

public interface IGameRenderer
{
    public void Render(Tilemap tilemap);
}

public class GameRenderer : IGameRenderer
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;
    private RenderTarget2D _offScreenRenderTarget;

    private readonly IEntityService _entityService;

    private readonly IGameSettings _gameSettings;

    public GameRenderer(
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        IEntityService entityService,
        IGameSettings gameSettings
    )
    {
        _gameSettings = gameSettings;

        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;

        _entityService = entityService;

        _offScreenRenderTarget = new RenderTarget2D(
            graphicsDevice,
            _gameSettings.InternalSize.Width,
            _gameSettings.InternalSize.Height
        );
    }

    public void Render(Tilemap tilemap)
    {
        _graphicsDevice.SetRenderTarget(_offScreenRenderTarget);
        _graphicsDevice.Clear(Color.Transparent);

        _spriteBatch.Begin();

        DrawToOffScreenRenderTarget(tilemap);

        _spriteBatch.End();

        _graphicsDevice.SetRenderTarget(null);

        DrawRenderTargetToScreen();
    }

    /// <summary>
    /// Draw everything to an off-screen render target.
    /// This allows us to apply scaling and other effects before drawing to the back buffer (the screen).
    /// </summary>
    private void DrawToOffScreenRenderTarget(Tilemap tilemap)
    {
        tilemap.Draw();

        // Get renderable entities
        var renderableEntities = _entityService.GetRenderables();

        foreach (var entity in renderableEntities)
        {
            entity.Draw();
        }

        // sprite.Draw(position: new Vector2(100, 100), origin: new Vector2(8, 8));
    }

    /// <summary>
    /// Draw the off-screen render targets to the back buffer, applying any necessary scaling.
    /// In other words, draw the contents of our internal render targets to the screen.
    /// </summary>
    private void DrawRenderTargetToScreen()
    {
        _spriteBatch.Begin(
           SpriteSortMode.Immediate,
           BlendState.AlphaBlend,
           SamplerState.PointClamp,
           null,
           null
       );

        _spriteBatch.Draw(
            _offScreenRenderTarget,
            new Rectangle(0, 0, _gameSettings.WindowSize.Width, _gameSettings.WindowSize.Height),
            Color.White
        );

        _spriteBatch.End();
    }
}



