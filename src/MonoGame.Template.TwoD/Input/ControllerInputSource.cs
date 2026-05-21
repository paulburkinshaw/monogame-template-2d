using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Template.TwoD.Input;

public class ControllerInputSource : IInputSource
{
    public bool MoveLeft { get; }

    public bool MoveRight { get; }

    public bool MoveUp { get; }

    public bool MoveDown { get; }

    public void ProcessInput()
    {
       // Implement controller input processing here
    }
}
