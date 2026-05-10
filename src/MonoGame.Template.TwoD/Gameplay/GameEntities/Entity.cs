using System;

namespace MonoGame.Template.TwoD.Gameplay.GameEntities;

/// <summary>
/// Base class for all entities in the game (players, NPCs, items, etc.)
/// </summary>
public abstract class Entity : IEntity
{
    public Guid Id { get; }
    protected Entity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }
}
