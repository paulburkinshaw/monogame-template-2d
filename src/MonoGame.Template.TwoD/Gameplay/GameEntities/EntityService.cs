using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MonoGame.Template.TwoD.Gameplay.GameEntities;

public interface IEntityService
{
    IReadOnlyCollection<IEntity> GetAllEntities();
    IEntity? GetEntityById(Guid id);
    void Register(IEntity entity);
    bool Remove(Guid id);

    IReadOnlyCollection<IUpdatable> GetUpdatables();
    IReadOnlyCollection<IRenderable> GetRenderables();
}

public class EntityService : IEntityService
{
    private readonly Dictionary<Guid, IEntity> _entities = new();

    public EntityService()
    {
        _entities = [];
    }

    public IReadOnlyCollection<IEntity> GetAllEntities()
    {
        return new ReadOnlyCollection<IEntity>([.. _entities.Values]);
    }

    public IEntity? GetEntityById(Guid id)
    {
        return _entities.TryGetValue(id, out var entity) ? entity : null;
    }

    public void Register(IEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!_entities.TryAdd(entity.Id, entity))
        {
            throw new InvalidOperationException($"An entity with id '{entity.Id}' is already registered.");
        }
    }

    public bool Remove(Guid id)
    {
        var entity = _entities.TryGetValue(id, out var e) ? e : null;

        if (entity is null)
        {
            return false;
        }

        _entities.Remove(entity.Id);
        return true;
    }

    public IReadOnlyCollection<IUpdatable> GetUpdatables()
    {
        return _entities.Values
            .OfType<IUpdatable>()
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyCollection<IRenderable> GetRenderables()
    {
        return _entities.Values
            .OfType<IRenderable>()
            .ToList()
            .AsReadOnly();
    }
}

