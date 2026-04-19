namespace MonoGame.Template.TwoD.Core;

public class GameSettingsConfig
{
    public ResolutionConfig InternalSize { get; set; }
    public ResolutionConfig WindowSize { get; set; }
    public int TargetFramesPerSecond { get; set; }
}

public class ResolutionConfig
{
    public int Width { get; set; }
    public int Height { get; set; }
}