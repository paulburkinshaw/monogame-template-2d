using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Template.TwoD.Input;

public interface IInputSource
{
    void ProcessInput();
    bool MoveLeft { get; }
    bool MoveRight { get; }
    bool MoveUp { get; }
    bool MoveDown { get; }
}
