using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Template.TwoD.Core;

public interface IGameSettings
{
    Rectangle InternalSize { get; }
    Rectangle WindowSize { get; }
    AnimationSettings AnimationSettings { get; }
    TilemapSettings TilemapSettings { get; }
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

/// <summary>
/// GameSettings holds immutable configuration data for the game.
/// </summary>
public class GameSettings : IGameSettings
{
    private readonly Rectangle _internalSize;
    private readonly Rectangle _windowSize;
    private readonly AnimationSettings _animationSettings;
    private readonly TilemapSettings _tilemapSettings;

    public GameSettings(
        Rectangle internalSize,
        Rectangle windowSize,
        AnimationSettings animationSettings,
        TilemapSettings tilemapSettings
        )
    {
        _internalSize = internalSize;
        _windowSize = windowSize;

        _animationSettings = animationSettings;

        _tilemapSettings = tilemapSettings;
    }

    public Rectangle InternalSize => _internalSize;
    public Rectangle WindowSize => _windowSize;
    public AnimationSettings AnimationSettings => _animationSettings;
    public TilemapSettings TilemapSettings => _tilemapSettings;
}
