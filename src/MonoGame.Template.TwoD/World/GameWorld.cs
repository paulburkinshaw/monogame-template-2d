using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MonoGame.Template.TwoD.Gameplay.GameEntities;
using MonoTiled;

namespace MonoGame.Template.TwoD.World;

public class GameWorld : IGameWorld
{
    private IList<Tilemap> _tilemaps = new List<Tilemap>();
    private Tilemap? _activeTilemap;

    public GameWorld(IEntityService entityService)
    {
        EntityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
        Tilemaps = new ReadOnlyCollection<Tilemap>(_tilemaps);
    }

    public IEntityService EntityService { get; }

    public IReadOnlyCollection<Tilemap> Tilemaps { get; }

    public Tilemap ActiveTilemap => _activeTilemap ?? throw new InvalidOperationException("The active tilemap has not been set. Load content and assign an active tilemap before rendering the game world");

    public void AddTilemap(Tilemap tilemap)
    {
        ArgumentNullException.ThrowIfNull(tilemap);

        _tilemaps.Add(tilemap);
        _activeTilemap = tilemap;
    }

    public void AddEntity(IEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        EntityService.Register(entity);
    }
}