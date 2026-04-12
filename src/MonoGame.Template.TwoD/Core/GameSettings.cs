using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Template.TwoD.Core;

public interface IGameSettings
{

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
    public float GetFrameDuration()
    {
        return 1 / _framesPerSecond * 1000f;
    }
}

public class GameSettings : IGameSettings
{
    private Rectangle _internalSize;
    private Rectangle _windowSize;
    private TimeSpan _targetElapsedTime;
    private AnimationSettings _animationSettings;

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
    public float ElapsedGameTimeSeconds { get; set; }
    public float ElapsedGameTimeMs { get; set; }
    public Rectangle InternalSize => _internalSize;
    public Rectangle WindowSize => _windowSize;
    public AnimationSettings AnimationSettings => _animationSettings;
}
