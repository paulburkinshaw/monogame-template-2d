using Microsoft.Xna.Framework.Input;

namespace MonoGame.Template.TwoD.Input;

public class KeyboardInputSource : IInputSource
{
    private InputManager _input;

    Keys _leftKey;
    Keys _rightKey;
    Keys _upKey;
    Keys _downKey;

    // Add more keys here

    public KeyboardInputSource(
       Keys leftKey,
       Keys rightKey,
       Keys upKey,
       Keys downKey
       )
    {
        _input = TemplateGame.Input;

        _leftKey = leftKey;
        _rightKey = rightKey;
        _upKey = upKey;
        _downKey = downKey;
    }

    public bool MoveLeft { get; private set; }

    public bool MoveRight { get; private set; }

    public bool MoveUp { get; private set; }

    public bool MoveDown { get; private set; }

    public void ProcessInput()
    {
        ProcessKeyboardState();
    }

    private void ProcessKeyboardState()
    {       
        MoveLeft = _input.Keyboard.IsKeyDown(_leftKey);
        MoveRight = _input.Keyboard.IsKeyDown(_rightKey);
        MoveUp = _input.Keyboard.IsKeyDown(_upKey);
        MoveDown = _input.Keyboard.IsKeyDown(_downKey);
    }
}
