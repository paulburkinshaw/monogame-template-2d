using MonoGame.Template.TwoD.Input;

namespace MonoGame.Template.TwoD.Gameplay.GameEntities;

public interface IHasInputSource
{
    public IInputSource InputSource { get; }
}