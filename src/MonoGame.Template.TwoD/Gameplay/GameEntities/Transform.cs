using Microsoft.Xna.Framework;

namespace MonoGame.Template.TwoD.Gameplay.GameEntities;

/// <summary>
/// Represents spatial state of a game entity, such as position, rotation, and scale.
/// This class owns the position and rotation of the entity, and is used by the rendering system to draw the entity at the correct location and orientation.
/// </summary>
public sealed class Transform
{
    public Vector2 Position { get; set; }

    /// <summary>
    /// Origin for rotation and scaling, typically the center of the sprite
    /// </summary>
    public Vector2 Origin { get; set; }

    /// <summary>
    /// Rotation in radians
    /// Note: MonoGame's SpriteBatch.Draw method uses rotation in radians, so this property is designed to be compatible with that. 
    /// You can convert degrees to radians using MathHelper.ToRadians(degrees).
    /// 0 = pointing up
    /// 90 = pointing right
    /// 180 = pointing down
    /// 270 = pointing left
    /// For example, to rotate an entity 90 degrees to the right, you would set Rotation to MathHelper.ToRadians(90).
    /// </summary>
    public float Rotation { get; set; }

    public Transform(Vector2 position, Vector2 origin)
    {
        Position = position;
        Origin = origin;
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }
}
