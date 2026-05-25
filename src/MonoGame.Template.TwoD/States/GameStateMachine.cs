using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Template.TwoD.States;

public sealed class GameStateMachine
{
    private IGameState? _currentState;

    public IGameState? CurrentState => _currentState;

    public GameStateMachine(IGameState initialState)
    {
        ArgumentNullException.ThrowIfNull(initialState);
        ChangeState(initialState);
    }

    public void ChangeState(IGameState next)
    {
        _currentState?.Exit();
        _currentState = next;
        _currentState.Enter(this);
    }

    public void Update(GameTime gameTime) => _currentState?.Update(gameTime);
    public void Draw(GameTime gameTime) => _currentState?.Draw(gameTime);
}
