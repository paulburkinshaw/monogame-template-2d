using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Template.TwoD.Rendering;
using System;

namespace MonoGame.Template.TwoD.States;

public class MenuState : IGameState
{
    private GameStateMachine _stateMachine = null!;
    private IGameState _nextState;
    private IUIRenderer _uIRenderer;

    public MenuState(
        IGameState nextState,
        IUIRenderer uIRenderer
        )
    {
        ArgumentNullException.ThrowIfNull(nextState);
        ArgumentNullException.ThrowIfNull(uIRenderer);

        _nextState = nextState;
        _uIRenderer = uIRenderer;
    }

    public void Enter(GameStateMachine stateMachine)
    {
        ArgumentNullException.ThrowIfNull(stateMachine);

        _stateMachine = stateMachine;
        // Window.Title = "Menu";
        // set up input bindings, reset menu selection, etc.
    }

    public void Exit()
    {
        // unsubscribe events, cleanup transient state
    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        // TODO: handle exiting the game from the menu
        //if (keyboardState.IsKeyDown(Keys.Escape))
        //    Exit();

        if (keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Enter))
            _stateMachine.ChangeState(_nextState);
    }

    public void Draw(GameTime gameTime)
    {
        // draw menu UI
        _uIRenderer.Render();
    }
}
