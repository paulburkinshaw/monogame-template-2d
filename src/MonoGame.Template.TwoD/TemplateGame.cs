using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Template.TwoD.Core;
using MonoGame.Template.TwoD.Gameplay.GameEntities;
using MonoGame.Template.TwoD.Input;
using MonoGame.Template.TwoD.Rendering;
using MonoGame.Template.TwoD.World;
using MonoSprite;
using MonoSprite.Converters;
using MonoTiled;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Text.Json;
using TiledDotNet;
using TiledDotNet.Converters;

namespace MonoGame.Template.TwoD;

public class TemplateGame : Game
{
    private const string GAMESETTINGS_FILE = "gameSettings.json";

    private IServiceProvider _serviceProvider;

    private GraphicsDeviceManager _graphicsDeviceManager;
    // One SpriteBatch instance will be used for the whole game
    private SpriteBatch _spriteBatch;
    private ISpriteService _spriteService;

    private ITilemapService _tilemapService;

    private IGameRenderer _gameRenderer;

    private IGameSettings _gameSettings;

    private GameWorld _gameWorld;

    public TemplateGame()
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "MonoGame.Template.TwoD.Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        var serviceCollection = new ServiceCollection();

        var gameSettings = GetGameSettings();
        serviceCollection.AddSingleton<IGameSettings>(gameSettings);

        serviceCollection.AddSingleton(GraphicsDevice);
        serviceCollection.AddSingleton<SpriteBatch>();

        serviceCollection.AddSingleton<IFileSystem, FileSystem>();

        // Sprite services
        serviceCollection.AddSingleton<IAsepriteSpritesheetJsonConverterService, AsepriteSpritesheetJsonConverterService>();
        serviceCollection.AddSingleton<IAsepriteSpritesheetService, AsepriteSpritesheetService>();
        serviceCollection.AddSingleton<ISpriteService, SpriteService>(sp =>
           new SpriteService(
                asepriteSpritesheetService: sp.GetRequiredService<IAsepriteSpritesheetService>(),
                content: Content,
                graphicsDevice: sp.GetRequiredService<GraphicsDevice>(),
                spriteBatch: sp.GetRequiredService<SpriteBatch>()
        ));

        serviceCollection.AddSingleton<IEntityService, EntityService>();

        // Tilemap services
        serviceCollection.AddSingleton<ITiledTilemapJsonConverterService, TiledTilemapJsonConverterService>();
        serviceCollection.AddSingleton<ITiledTilemapService, TiledTilemapService>();
        // Provides tile colour data for pixel-perfect collision detection
        serviceCollection.AddSingleton<ITilesetTextureService, TilesetTextureService>();
        serviceCollection.AddSingleton<ITilemapService, TilemapService>(sp =>
            new TilemapService(
                tiledTilemapService: sp.GetRequiredService<ITiledTilemapService>(),
                tilesetTextureService: sp.GetRequiredService<ITilesetTextureService>(),
                content: Content,
                spriteBatch: sp.GetRequiredService<SpriteBatch>()
        ));

        serviceCollection.AddSingleton<IGameRenderer>(sp =>
            new GameRenderer(
               graphicsDevice: sp.GetRequiredService<GraphicsDevice>(),
               spriteBatch: sp.GetRequiredService<SpriteBatch>(),
               gameSettings: sp.GetRequiredService<IGameSettings>(),
               gameWorld: sp.GetRequiredService<IGameWorld>()
            )
        );

        // Register the concrete GameWorld for TemplateGame ownership/mutation
        // then expose the same singleton as IGameWorld for read-only consumers such as the renderer
        serviceCollection.AddSingleton<GameWorld>();

        serviceCollection.AddSingleton<IGameWorld, GameWorld>(sp =>
           sp.GetRequiredService<GameWorld>()
        );

        // Build the service provider
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _gameRenderer = _serviceProvider.GetRequiredService<IGameRenderer>();
        _gameSettings = _serviceProvider.GetRequiredService<IGameSettings>();

        _spriteBatch = _serviceProvider.GetRequiredService<SpriteBatch>();
        _spriteService = _serviceProvider.GetRequiredService<ISpriteService>();

        _tilemapService = _serviceProvider.GetRequiredService<ITilemapService>();

        _gameWorld = _serviceProvider.GetRequiredService<GameWorld>();

        _graphicsDeviceManager.IsFullScreen = false;
        _graphicsDeviceManager.PreferredBackBufferWidth = _gameSettings.WindowSize.Width;
        _graphicsDeviceManager.PreferredBackBufferHeight = _gameSettings.WindowSize.Height;
        _graphicsDeviceManager.ApplyChanges();
        _graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;

        IsFixedTimeStep = true;
        TargetElapsedTime = _gameSettings.AnimationSettings.TargetElapsedTime;

        base.Initialize();
    }

    protected override void LoadContent()
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
       
        _gameWorld.EntityService.Register(player1);

        var tilemapFilePath = @"Content\Tilemaps\tilemap1.tmj";
        var tilemap = _tilemapService.Load(
           tilemapFilepath: tilemapFilePath,
           tilesetFilepath: "Tilesets"
        );

        _gameWorld.AddTilemap(tilemap);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var entityService = _gameWorld.EntityService;
         
        // process input for entities that have input source (ie player entities)
        var entitiesWithInputSource = entityService.GetEntitiesWithInputSource();
        foreach (var entity in entitiesWithInputSource)
        {
            entity.InputSource.ProcessInput();
        }

        // Update all updateable entities
        var updateableEntities = entityService.GetUpdatables();       
        foreach (var entity in updateableEntities)
        {
            entity.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _gameRenderer.Render();

        base.Draw(gameTime);
    }

    /// <summary>
    /// Loads and returns the game settings from a configuration file.
    /// </summary>
    /// <remarks>The method reads the game settings from a JSON configuration file located in the
    /// application's base directory.</remarks>
    /// <returns>A <see cref="GameSettings"/> object containing the loaded game settings.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the configuration file is not found at the expected path.</exception>
    private static GameSettings GetGameSettings()
    {
        var settingsPath = Path.Combine(AppContext.BaseDirectory, GAMESETTINGS_FILE);
        if (!File.Exists(settingsPath))
        {
            throw new FileNotFoundException(
                $"Settings file '{GAMESETTINGS_FILE}' was not found at '{settingsPath}'.",
                settingsPath);
        }

        var json = File.ReadAllText(settingsPath);
        var gameSettingsConfig = JsonSerializer.Deserialize<GameSettingsConfig>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return new GameSettings(
            internalSize: new Rectangle(
                0, 0,
                gameSettingsConfig.InternalSize.Width,
                gameSettingsConfig.InternalSize.Height),
            windowSize: new Rectangle(
                0, 0,
                gameSettingsConfig.WindowSize.Width,
                gameSettingsConfig.WindowSize.Height),
            animationSettings: new AnimationSettings(gameSettingsConfig.AnimationSettings.TargetFramesPerSecond),
            tilemapSettings: new TilemapSettings((TilemapType)gameSettingsConfig.TilemapSettings.TilemapType)
        );
    }
}
