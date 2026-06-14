using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Template.TwoD.Gameplay.GameEntities;
using MonoGame.Template.TwoD.Input;
using MonoGame.Template.TwoD.Rendering;
using MonoGame.Template.TwoD.World;
using MonoSprite;
using MonoTiled;
using System;

namespace MonoGame.Template.TwoD.Scenes;

public class GameplayScene : IScene
{
    private SpriteBatch _spriteBatch;
    private ISpriteService _spriteService;
    private ITilemapService _tilemapService;
    private SceneManager _sceneManager = null!;
    private GameWorld _gameWorld;
    private IGameRenderer _gameRenderer;
    private bool _isDisposed;

    public GameplayScene()
    {
        _spriteBatch = TemplateGame.SpriteBatch;
        _spriteService = TemplateGame.SpriteService;
        _tilemapService = TemplateGame.TilemapService;
        _gameWorld = TemplateGame.ConcreteGameWorld;
        _gameRenderer = TemplateGame.GameRenderer;

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

        // set up input bindings, initialize player entities, etc.
    }

    public void LoadContent()
    {
        var playerSprite = _spriteService.CreateSpriteInstance(
             spritesheetFilepath: @"Spritesheets",
             spritesheetName: "player1-spritesheet",
             initialAnimationName: "Landed",
             _spriteBatch);

        // This could come from a main menu selection
        var inputSource = new KeyboardInputSource(
            leftKey: Keys.A,
            rightKey: Keys.D,
            upKey: Keys.W,
            downKey: Keys.X
        );

        var player1 = new Player(
         sprite: playerSprite,
         transform: new Transform(
             position: new Vector2(40, 230),
             origin: new Vector2(8, 8)
         ),
         inputSource: inputSource
        );
   
        _gameWorld.AddEntity(player1);

        var tilemapFilePath = @"Content\Tilemaps\tilemap1.tmj";
        var tilemap = _tilemapService.Load(
           tilemapFilepath: tilemapFilePath,
           tilesetFilepath: "Tilesets"
        );

        _gameWorld.AddTilemap(tilemap);
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
        var entityService = _gameWorld.EntityService;

        // Update all updateable entities
        var updateableEntities = entityService.GetUpdatables();
        foreach (var entity in updateableEntities)
        {
            entity.Update(gameTime);
        }
    }

    public void Draw(GameTime time)
    {
        _gameRenderer.Render();
    }

    // Finalizer, called when object is cleaned up by garbage collector.
    ~GameplayScene() => Dispose(false);

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
