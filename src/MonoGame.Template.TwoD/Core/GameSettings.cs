using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGame.Template.TwoD.Core;

public interface IGameSettings
{
    string Language { get; }
    Rectangle InternalSize { get; }
    Rectangle WindowSize { get; }
    AnimationSettings AnimationSettings { get; }
    TilemapSettings TilemapSettings { get; }
    UISettings UISettings { get; }
    ContentSettings ContentSettings { get; }
}

public class AnimationSettings
{
    private readonly TimeSpan _targetElapsedTime;
    private float _targetFramesPerSecond;

    public AnimationSettings(
       int targetFramesPerSecond = 30
       )
    {
        _targetFramesPerSecond = targetFramesPerSecond;

        // Calculate target elapsed time based on target frames per second
        // For example, if targetFramesPerSecond is 30, targetElapsedTime will be 33ms
        // this is because 1 second / 30 frames = 0.0333 seconds per frame = 33.33ms per frame
        _targetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond / targetFramesPerSecond));
    }

    public TimeSpan TargetElapsedTime => _targetElapsedTime;

    public float TargetFramesPerSecond => _targetFramesPerSecond;

    /// <summary>
    /// Returns frame duration in milliseconds
    /// </summary>
    /// <returns></returns>
    public float GetFrameDurationMs()
    {
        return 1 / _targetFramesPerSecond * 1000f;
    }
}

public enum TilemapType
{
    Static,
    Dynamic
}

public class TilemapSettings
{
    private TilemapType _tilemapType;

    public TilemapSettings(
       TilemapType tilemapType = TilemapType.Dynamic // Default to Static once we support static tilemaps
       )
    {
        _tilemapType = tilemapType;
    }

    public TilemapType TilemapType => _tilemapType;
}

public class UISettings
{
    private string _menuFontName;

    public UISettings(
       string menuFontName
       )
    {
        ArgumentNullException.ThrowIfNullOrEmpty(menuFontName);
        _menuFontName = menuFontName;
    }

    public string MenuFontName => _menuFontName;
}

public class ContentSettings
{
    private Dictionary<string, LanguageContent> _languages;

    public ContentSettings(
       Dictionary<string, LanguageContent> languages    
       )
    {
        ArgumentNullException.ThrowIfNull(languages);
        _languages = languages;
    }

    public LanguageContent GetForLanguage(string language) =>
    _languages.TryGetValue(language, out var languageContent) ? languageContent
    : throw new InvalidOperationException($"No content for language '{language}'.");
}

public class LanguageContent
{
    private string _menuScreenMessage;

    public LanguageContent(
       string menuScreenMessage
       )
    {
        _menuScreenMessage = menuScreenMessage;
    }

    public string MenuScreenMessage => _menuScreenMessage;
}

/// <summary>
/// GameSettings holds immutable configuration data for the game.
/// </summary>
public class GameSettings : IGameSettings
{
    private readonly string _language;
    private readonly Rectangle _internalSize;
    private readonly Rectangle _windowSize;
    private readonly AnimationSettings _animationSettings;
    private readonly TilemapSettings _tilemapSettings;
    private readonly UISettings _uISettings;
    private readonly ContentSettings _contentSettings;

    public GameSettings(
        string language,
        Rectangle internalSize,
        Rectangle windowSize,
        AnimationSettings animationSettings,
        TilemapSettings tilemapSettings,
        UISettings uISettings,
        ContentSettings contentSettings
        )
    {
        _language = language;

        _internalSize = internalSize;
        _windowSize = windowSize;

        _animationSettings = animationSettings;

        _tilemapSettings = tilemapSettings;

        _uISettings = uISettings;

        _contentSettings = contentSettings;
    }

    public string Language => _language;
    public Rectangle InternalSize => _internalSize;
    public Rectangle WindowSize => _windowSize;
    public AnimationSettings AnimationSettings => _animationSettings;
    public TilemapSettings TilemapSettings => _tilemapSettings;
    public UISettings UISettings => _uISettings;
    public ContentSettings ContentSettings => _contentSettings;
}
