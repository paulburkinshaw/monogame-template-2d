namespace MonoGame.Template.TwoD.Core;

/// <summary>
/// Used primarily for deserializing gameSettings.json to GameSettings
/// </summary>
public class GameSettingsConfig
{
    public ResolutionConfig InternalSize { get; set; }
    public ResolutionConfig WindowSize { get; set; }

    public AnimationSettingsConfig AnimationSettings { get; set; }
    public TilemapSettingsConfig TilemapSettings { get; set; }
}

public class ResolutionConfig
{
    public int Width { get; set; }
    public int Height { get; set; }
}

public class AnimationSettingsConfig
{
    public int TargetFramesPerSecond { get; set; }
}

public class TilemapSettingsConfig
{
    public int TilemapType { get; set; }
}