using Microsoft.Xna.Framework;
using MonoSprite;
using System;

namespace MonoGame.Template.TwoD.Gameplay.GameEntities;

public sealed class Player : Entity, IUpdatable, IRenderable, IHasTransform
{
    private readonly Sprite _sprite;

    public Transform Transform { get; }

    public Player(
        Sprite sprite,
        Transform transform,
        Guid id = default)
        : base(id)
    {
        _sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
        Transform = transform ?? throw new ArgumentNullException(nameof(transform));
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw()
    {
        _sprite.Draw(
            Transform.Position,
            rotation: Transform.Rotation,
            origin: Transform.Origin);
    }
}
