using Microsoft.Xna.Framework.Input;

namespace MonoGame.Template.TwoD.Input;

public class KeyboardInputSource : IInputSource
{
    private KeyboardState _keyboardStateOld = Keyboard.GetState();

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
        var keyboardState = Keyboard.GetState();

        // Reset flags each frame
        MoveLeft = false;
        MoveRight = false;
        MoveUp = false;
        MoveDown = false;

        MoveLeft = keyboardState.IsKeyDown(_leftKey);
        MoveRight = keyboardState.IsKeyDown(_rightKey);
        MoveUp = keyboardState.IsKeyDown(_upKey);
        MoveDown = keyboardState.IsKeyDown(_downKey);

        // _keyboardStateOld is available here for "just pressed" detection if needed
        // e.g. bool jumpJustPressed = keyboardState.IsKeyDown(_jumpKey) && _keyboardStateOld.IsKeyUp(_jumpKey);

        // Set the old keyboard state ready for the next frame
        _keyboardStateOld = keyboardState;
    }
}
