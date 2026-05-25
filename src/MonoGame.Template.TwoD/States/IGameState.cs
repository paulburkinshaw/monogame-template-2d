using Microsoft.Xna.Framework;

namespace MonoGame.Template.TwoD.States;

public interface IGameState
{
    // Called when the state is entered, used for initialization and setup
    void Enter(GameStateMachine stateMachine);
    // Called when the state is exited, used for cleanup and resource management
    void Exit();

    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);
}
