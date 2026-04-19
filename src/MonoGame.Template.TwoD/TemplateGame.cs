using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Template.TwoD.Core;
using MonoGame.Template.TwoD.Rendering;
using MonoSprite;
using MonoSprite.Converters;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Text.Json;

namespace MonoGame.Template.TwoD;

public class TemplateGame : Game
{
    private const string GAMESETTINGS_FILE = "gameSettings.json";

    private IServiceProvider _serviceProvider;

    private GraphicsDeviceManager _graphicsDeviceManager;
    // One SpriteBatch instance will be used for the whole game
    private SpriteBatch _spriteBatch;
    private ISpriteService _spriteService;
    private Sprite _sprite1;

    private IGameRenderer _gameRenderer;

    private IGameSettings _gameSettings;

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

        serviceCollection.AddSingleton<IAsepriteSpritesheetJsonConverterService, AsepriteSpritesheetJsonConverterService>();
        serviceCollection.AddSingleton<IAsepriteSpritesheetService, AsepriteSpritesheetService>();
        serviceCollection.AddSingleton<ISpriteService, SpriteService>(sp =>
           new SpriteService(
            asepriteSpritesheetService: sp.GetRequiredService<IAsepriteSpritesheetService>(),
            content: Content,
            graphicsDevice: sp.GetRequiredService<GraphicsDevice>(),
            spriteBatch: sp.GetRequiredService<SpriteBatch>()
        ));

        serviceCollection.AddSingleton<IGameRenderer>(sp =>
            new GameRenderer(
               graphicsDevice: sp.GetRequiredService<GraphicsDevice>(),
               spriteBatch: sp.GetRequiredService<SpriteBatch>(),
               spriteService: sp.GetRequiredService<ISpriteService>(),
               gameSettings: sp.GetRequiredService<IGameSettings>()
            )
        );

        // Build the service provider
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _gameRenderer = _serviceProvider.GetRequiredService<IGameRenderer>();
        _gameSettings = _serviceProvider.GetRequiredService<IGameSettings>();

        _spriteBatch = _serviceProvider.GetRequiredService<SpriteBatch>();
        _spriteService = _serviceProvider.GetRequiredService<ISpriteService>();

        _graphicsDeviceManager.IsFullScreen = false;
        _graphicsDeviceManager.PreferredBackBufferWidth = _gameSettings.WindowSize.Width;
        _graphicsDeviceManager.PreferredBackBufferHeight = _gameSettings.WindowSize.Height;
        _graphicsDeviceManager.ApplyChanges();
        _graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;

        IsFixedTimeStep = true;
        TargetElapsedTime = _gameSettings.TargetElapsedTime;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Load sprites
        _sprite1 = _spriteService.CreateSpriteInstance(
            spritesheetFilepath: @"Spritesheets",
            spritesheetName: "player1-spritesheet",
            initialAnimationName: "Landed",
            _spriteBatch,
            _gameSettings.WindowSize);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _gameRenderer.Render(gameTime);

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
        var settingsConfig = JsonSerializer.Deserialize<GameSettingsConfig>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return new GameSettings(
            internalSize: new Rectangle(
                0, 0,
                settingsConfig.InternalSize.Width,
                settingsConfig.InternalSize.Height),
            windowSize: new Rectangle(
                0, 0,
                settingsConfig.WindowSize.Width,
                settingsConfig.WindowSize.Height),
            animationSettings: new AnimationSettings(),
            targetFramesPerSecond: settingsConfig.TargetFramesPerSecond);
    }
}
