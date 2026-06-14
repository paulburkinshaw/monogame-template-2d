using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Template.TwoD.Scenes;

public sealed class SceneManager
{
    private IScene? _activeScene;
    private IScene? _nextScene;

    public IScene? ActiveScene => _activeScene;

    public SceneManager(IScene initialScene)
    {
        ArgumentNullException.ThrowIfNull(initialScene);
        ChangeScene(initialScene);
    }

    public void Update(GameTime gameTime)
    {
        // if there is a next scene waiting to be switch to, then transition
        // to that scene.
        if (_nextScene != null)
            TransitionScene();

        // If there is an active scene, update it.
        if (_activeScene != null)
            _activeScene.Update(gameTime);
    }
    public void Draw(GameTime gameTime)
    {

        _activeScene?.Draw(gameTime);
    }

    public void ChangeScene(IScene next)
    {
        // Only set the next scene value if it is not the same
        // instance as the currently active scene.
        if (_activeScene != next)
            _nextScene = next;      
    }

    private void TransitionScene()
    {
        // If there is an active scene, dispose of it.
        if (_activeScene != null)
            _activeScene.Dispose();

        // Force the garbage collector to collect to ensure memory is cleared.
        GC.Collect();

        // Change the currently active scene to the new scene.
        _activeScene = _nextScene;

        // Null out the next scene value so it does not trigger a change over and over.
        _nextScene = null;

        // If the active scene now is not null, initialize it.
        // Remember, just like with Game, the Initialize call also calls the
        // Scene.LoadContent
        if (_activeScene != null)
            _activeScene.Initialize(this);
    }
}
