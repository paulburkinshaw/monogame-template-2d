using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Template.TwoD.Core;
using MonoGame.Template.TwoD.Gameplay.GameEntities;
using MonoGame.Template.TwoD.Input;
using MonoGame.Template.TwoD.Rendering;
using MonoGame.Template.TwoD.UI;
using MonoGame.Template.TwoD.World;
using MonoGame.Template.TwoD.Scenes;
using MonoSprite;
using MonoSprite.Converters;
using MonoTiled;
using System;
using System.Collections.Generic;
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
    private SceneManager _sceneManager;

    public static IGameSettings GameSettings { get; private set; }
    public static new ContentManager Content { get; private set; }
    public static IGameWorld GameWorld { get; private set; }
    public static GameWorld ConcreteGameWorld { get; private set; }
    public static IGameRenderer GameRenderer { get; private set; }
    public static IUIRenderer UIRenderer { get; private set; }
    public static IUIService UIService { get; private set; }
    public static ISpriteService SpriteService { get; private set; }
    public static ITilemapService TilemapService { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    public static InputManager Input { get; private set; }

    public TemplateGame()
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this);

        // Set the content manager to a reference of the base Game's
        // content manager.
        Content = base.Content;
        Content.RootDirectory = "MonoGame.Template.TwoD.Content";

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        var serviceCollection = new ServiceCollection();

        var gameSettings = GetGameSettings();
        serviceCollection.AddSingleton<IGameSettings>(gameSettings);

        serviceCollection.AddSingleton(GraphicsDevice);
        // One SpriteBatch instance will be used for the whole game
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
        serviceCollection.AddSingleton<IUIService, UIService>();

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

        serviceCollection.AddSingleton<IUIRenderer>(sp =>
            new UIRenderer(
               graphicsDevice: sp.GetRequiredService<GraphicsDevice>(),
               spriteBatch: sp.GetRequiredService<SpriteBatch>(),
               gameSettings: sp.GetRequiredService<IGameSettings>(),
               uIService: sp.GetRequiredService<IUIService>()
            )
        );

        // Register the concrete GameWorld for GameplayScene ownership/mutation
        // then expose the same singleton as IGameWorld for read-only consumers such as the renderer
        serviceCollection.AddSingleton<GameWorld>(sp =>
            new GameWorld(
                entityService: sp.GetRequiredService<IEntityService>()
            )
        );

        serviceCollection.AddSingleton<IGameWorld, GameWorld>(sp =>
           sp.GetRequiredService<GameWorld>()
        );

        // Register SceneManager
        serviceCollection.AddSingleton<SceneManager>(sp =>
            new SceneManager(
                initialScene: new MenuScene()
            )
        );

        // Build the service provider
        _serviceProvider = serviceCollection.BuildServiceProvider();

        GameSettings = _serviceProvider.GetRequiredService<IGameSettings>();

        SpriteBatch = _serviceProvider.GetRequiredService<SpriteBatch>();
        SpriteService = _serviceProvider.GetRequiredService<ISpriteService>();
        TilemapService = _serviceProvider.GetRequiredService<ITilemapService>();

        ConcreteGameWorld = _serviceProvider.GetRequiredService<GameWorld>();
        GameWorld = _serviceProvider.GetRequiredService<IGameWorld>();

        GameRenderer = _serviceProvider.GetRequiredService<IGameRenderer>();
        UIRenderer = _serviceProvider.GetRequiredService<IUIRenderer>();

        UIService = _serviceProvider.GetRequiredService<IUIService>();

        _sceneManager = _serviceProvider.GetRequiredService<SceneManager>();

        _graphicsDeviceManager.IsFullScreen = false;
        _graphicsDeviceManager.PreferredBackBufferWidth = GameSettings.WindowSize.Width;
        _graphicsDeviceManager.PreferredBackBufferHeight = GameSettings.WindowSize.Height;
        _graphicsDeviceManager.ApplyChanges();
        _graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;

        IsFixedTimeStep = true;
        TargetElapsedTime = GameSettings.AnimationSettings.TargetElapsedTime;

        // Create a new input manager.
        Input = new InputManager();

        base.Initialize();
    }

    protected override void LoadContent()
    {


    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update(gameTime);

        // Each scene is responsible for processing input, updating entities, game logic, UI, etc relevant to that scene 
        // so we call update on the current scene.
        _sceneManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Each scene is responsible for drawing relevant entities or UI for that scene
        // so we call draw on the current scene.
        _sceneManager.Draw(gameTime);

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
            language: gameSettingsConfig.Language,
            internalSize: new Rectangle(
                0, 0,
                gameSettingsConfig.InternalSize.Width,
                gameSettingsConfig.InternalSize.Height),
            windowSize: new Rectangle(
                0, 0,
                gameSettingsConfig.WindowSize.Width,
                gameSettingsConfig.WindowSize.Height),
            animationSettings: new AnimationSettings(gameSettingsConfig.AnimationSettings.TargetFramesPerSecond),
            tilemapSettings: new TilemapSettings((TilemapType)gameSettingsConfig.TilemapSettings.TilemapType),
            uISettings: new UISettings(
                gameSettingsConfig.UISettings.MenuFontName
            ),
            contentSettings: new ContentSettings(
              new Dictionary<string, LanguageContent>(
                gameSettingsConfig.ContentSettings.Languages,
                StringComparer.OrdinalIgnoreCase
              )
            )
        );
    }
}
