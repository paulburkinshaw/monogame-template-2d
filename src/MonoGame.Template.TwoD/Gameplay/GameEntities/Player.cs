using Microsoft.Xna.Framework;
using MonoGame.Template.TwoD.Input;
using MonoSprite;
using System;

namespace MonoGame.Template.TwoD.Gameplay.GameEntities;

public sealed class Player : Entity, IUpdatable, IRenderable, IHasTransform, IHasInputSource
{
    private readonly Sprite _sprite;

    public Transform Transform { get; private set; }

    public IInputSource InputSource { get; }

    public Player(
        Sprite sprite,
        Transform transform,
        IInputSource inputSource,
        Guid id = default)
        : base(id)
    {
        _sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
        Transform = transform ?? throw new ArgumentNullException(nameof(transform));
        InputSource = inputSource ?? throw new ArgumentNullException(nameof(inputSource));
    }

    public void Update(GameTime gameTime)
    {
        // Handle input
        if (InputSource.MoveLeft)
        {
            // Move player left        
            var newPosition = Transform.Position + new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            Transform.SetPosition(newPosition);
        }
       
        if (InputSource.MoveRight)
        {
            // Move player right        
            var newPosition = Transform.Position + new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            Transform.SetPosition(newPosition);
        }

        // Handle other input and game logic as needed

    }

    public void Draw()
    {
        _sprite.Draw(
            Transform.Position,
            rotation: Transform.Rotation,
            origin: Transform.Origin);
    }
}
