using System.Collections.Generic;
using MonoGame.Template.TwoD.Gameplay.GameEntities;
using MonoTiled;

namespace MonoGame.Template.TwoD.World;

public interface IGameWorld
{
    IEntityService EntityService { get; }
    IReadOnlyCollection<Tilemap> Tilemaps { get; }
    Tilemap ActiveTilemap { get; }
}