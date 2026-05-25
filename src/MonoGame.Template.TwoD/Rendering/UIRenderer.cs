using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Template.TwoD.Core;
using MonoGame.Template.TwoD.UI;
using System;

namespace MonoGame.Template.TwoD.Rendering;

public interface IUIRenderer
{
    public void Render();
}

public class UIRenderer : IUIRenderer
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;
    private RenderTarget2D _offScreenRenderTarget;
    private readonly IGameSettings _gameSettings;
    private readonly IUIService _uIService;
    private readonly LanguageContent _languageContent;

    public UIRenderer(
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        IGameSettings gameSettings,
        IUIService uIService
    )
    {
        _gameSettings = gameSettings;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _uIService = uIService;

        _offScreenRenderTarget = new RenderTarget2D(
            graphicsDevice,
            _gameSettings.InternalSize.Width,
            _gameSettings.InternalSize.Height
        );

        _languageContent = _gameSettings.ContentSettings.GetForLanguage(_gameSettings.Language);
    }

    public void Render()
    {
        DrawUIToOffScreenRenderTarget();

        DrawRenderTargetToScreen();
    }

    private void DrawUIToOffScreenRenderTarget()
    {
        var xCenter = (_gameSettings.InternalSize.Width / 2) - 100;
        var yCenter = (_gameSettings.InternalSize.Height / 2);

        _graphicsDevice.SetRenderTarget(_offScreenRenderTarget);
        _graphicsDevice.Clear(Color.Transparent);

        _spriteBatch.Begin();

        var menuFont = _uIService.GetSpriteFontByName(_gameSettings.UISettings.MenuFontName)
            ?? throw new InvalidOperationException($"Font '{_gameSettings.UISettings.MenuFontName}' has not been registered.");

        _spriteBatch.DrawString(menuFont, _languageContent.MenuScreenMessage, new Vector2(xCenter, yCenter), Color.White);   

        _spriteBatch.End();

        _graphicsDevice.SetRenderTarget(null);
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



