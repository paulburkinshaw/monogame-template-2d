using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Template.TwoD.Core;
using MonoGame.Template.TwoD.Rendering;
using MonoGame.Template.TwoD.UI;
using System;

namespace MonoGame.Template.TwoD.Scenes;

public class MenuScene : IScene
{
    private IGameSettings _gameSettings;
    private SceneManager _sceneManager = null!;
    private IUIRenderer _uIRenderer;
    private IUIService _uIService;
    private bool _isDisposed;

    public MenuScene()
    {
        _gameSettings = TemplateGame.GameSettings;
        _uIRenderer = TemplateGame.UIRenderer;
        _uIService = TemplateGame.UIService;

        Content = new ContentManager(TemplateGame.Content.ServiceProvider);
        // Set the root directory for content to the same as the root directory
        // for the game's content.
        Content.RootDirectory = TemplateGame.Content.RootDirectory;
    }

    /// <summary>
    /// Gets the ContentManager used for loading scene-specific assets.
    /// </summary>
    /// <remarks>
    /// Assets loaded through this ContentManager will be automatically unloaded when this scene ends.
    /// </remarks>
    public ContentManager Content { get; }

    /// <summary>
    /// Gets a value that indicates if the scene has been disposed of.
    /// </summary>
    public bool IsDisposed => _isDisposed;

    public void Initialize(SceneManager sceneManager)
    {
        ArgumentNullException.ThrowIfNull(sceneManager);

        _sceneManager = sceneManager;

        LoadContent();
        // Window.Title = "Menu";
        // set up input bindings, reset menu selection, etc.
    }

    public void LoadContent()
    {
        var menuFont = Content.Load<SpriteFont>(@$"Fonts\{_gameSettings.UISettings.MenuFontName}");
        _uIService.RegisterSpriteFont(menuFont, _gameSettings.UISettings.MenuFontName);
    }

    /// <summary>
    /// Unloads scene-specific content.
    /// </summary>
    public void UnloadContent()
    {
        Content.Unload();
    }

    public void Update(GameTime gameTime)
    {
        if (TemplateGame.Input.Keyboard.WasKeyJustPressed(Keys.Space) || TemplateGame.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            var gameplayScene = new GameplayScene();               
            _sceneManager.ChangeScene(gameplayScene);
        }
    }

    public void Draw(GameTime gameTime)
    {
        // draw menu UI
        _uIRenderer.Render();
    }

    // Finalizer, called when object is cleaned up by garbage collector.
    ~MenuScene() => Dispose(false);

    /// <summary>
    /// Disposes of this scene.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of this scene.
    /// </summary>
    /// <param name="disposing">'
    /// Indicates whether managed resources should be disposed.  This value is only true when called from the main
    /// Dispose method.  When called from the finalizer, this will be false.
    /// </param>
    public void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;

        if (disposing)
        {
            UnloadContent();
            Content.Dispose();
        }

        _isDisposed = true;
    }
}
