using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Template.TwoD.Core;

public interface IGameSettings
{
    TimeSpan TargetElapsedTime { get; }
    Rectangle InternalSize { get; }
    Rectangle WindowSize { get; }
    AnimationSettings AnimationSettings { get; }
}

public class AnimationSettings
{
    private float _framesPerSecond;

    public AnimationSettings(
       float framesPerSecond = 10f
       )
    {
        _framesPerSecond = framesPerSecond;
    }

    public float FramesPerSecond => _framesPerSecond;

    /// <summary>
    /// Returns frame duration in milliseconds
    /// </summary>
    /// <returns></returns>
    public float GetFrameDurationMs()
    {
        return 1 / _framesPerSecond * 1000f;
    }
}

public class GameSettings : IGameSettings
{
    private readonly Rectangle _internalSize;
    private readonly Rectangle _windowSize;
    private readonly TimeSpan _targetElapsedTime;
    private readonly AnimationSettings _animationSettings;

    public GameSettings(
        Rectangle internalSize,
        Rectangle windowSize, 
        AnimationSettings animationSettings,
        int targetFramesPerSecond = 30
        )
    {
        // Calculate target elapsed time based on target frames per second
        // For example, if targetFramesPerSecond is 30, targetElapsedTime will be 33ms
        // this is because 1 second / 30 frames = 0.0333 seconds per frame = 33.33ms per frame
        _targetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond / targetFramesPerSecond));

        _internalSize = internalSize;
        _windowSize = windowSize;

        _animationSettings = animationSettings;
    }

    public TimeSpan TargetElapsedTime => _targetElapsedTime;
    public Rectangle InternalSize => _internalSize;
    public Rectangle WindowSize => _windowSize;
    public AnimationSettings AnimationSettings => _animationSettings;
}
