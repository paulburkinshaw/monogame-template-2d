using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Template.TwoD.Rendering;
using MonoGame.Template.TwoD.World;
using System;

namespace MonoGame.Template.TwoD.States;

public class PlayingState : IGameState
{
    private GameStateMachine _stateMachine = null!;
    private IGameWorld _gameWorld;
    private IGameRenderer _gameRenderer;

    public PlayingState(
        IGameWorld gameWorld,
        IGameRenderer gameRenderer
        )
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
        _gameRenderer = gameRenderer ?? throw new ArgumentNullException(nameof(gameRenderer));
    }

    public void Enter(GameStateMachine stateMachine)
    {
        ArgumentNullException.ThrowIfNull(stateMachine);

        _stateMachine = stateMachine;
        // set up input bindings, initialize player entities, etc.
    }

    public void Exit()
    {
        // unsubscribe events, cleanup transient state
    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        // TODO: handle exiting the game from the playing state
        //if (keyboardState.IsKeyDown(Keys.Escape))
        //    Exit();

        var entityService = _gameWorld.EntityService;

        // process input for entities that have input source (ie player entities)
        var entitiesWithInputSource = entityService.GetEntitiesWithInputSource();
        foreach (var entity in entitiesWithInputSource)
        {
            entity.InputSource.ProcessInput();
        }

        // Update all updateable entities
        var updateableEntities = entityService.GetUpdatables();
        foreach (var entity in updateableEntities)
        {
            entity.Update(gameTime);
        }
    }

    public void Draw(GameTime time)
    {
        _gameRenderer.Render();
    }
}
