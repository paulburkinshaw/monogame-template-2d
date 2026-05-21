using Microsoft.Xna.Framework;
using MonoGame.Template.TwoD.Input;
using MonoSprite;
using System;

namespace MonoGame.Template.TwoD.Gameplay.GameEntities;

public sealed class Player : Entity, IUpdatable, IRenderable, IHasTransform, IHasInputSource
{
    private readonly Sprite _sprite;

    public Transform Transform { get; }

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
        }

    }

    public void Draw()
    {
        _sprite.Draw(
            Transform.Position,
            rotation: Transform.Rotation,
            origin: Transform.Origin);
    }
}
