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

    public GameWorld(IEntityService entities)
    {
        Entities = entities ?? throw new ArgumentNullException(nameof(entities));
        Tilemaps = new ReadOnlyCollection<Tilemap>(_tilemaps);
    }

    public IEntityService Entities { get; }

    public IReadOnlyCollection<Tilemap> Tilemaps { get; }

    public Tilemap ActiveTilemap => _activeTilemap ?? throw new InvalidOperationException("The active tilemap has not been set. Load content and assign an active tilemap before rendering the game world");

    public void AddTilemap(Tilemap tilemap)
    {
        ArgumentNullException.ThrowIfNull(tilemap);

        _tilemaps.Add(tilemap);
        _activeTilemap = tilemap;
    }
}