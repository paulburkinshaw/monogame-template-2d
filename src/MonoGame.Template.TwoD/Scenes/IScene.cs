using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MonoGame.Template.TwoD.Scenes;

public interface IScene : IDisposable
{
    public ContentManager Content { get; }

    public bool IsDisposed { get; }

    // Called when the scene is entered, used for initialization and setup
    void Initialize(SceneManager sceneManager);

    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);
}
