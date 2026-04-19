using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoTiled;
public interface ITilesetTextureService
{
    Texture2D GetTilesetTexture(int tilesetId);
    void AddTilesetTexture(int tilesetId, Texture2D texture);
}

public class TilesetTextureService : ITilesetTextureService
{
    private readonly Dictionary<int, Texture2D> _tilesetTextures = new();

    public Texture2D GetTilesetTexture(int tilesetId)
    {
        if (_tilesetTextures.TryGetValue(tilesetId, out var texture))
            return texture;
        throw new KeyNotFoundException($"Tileset texture for TilesetId {tilesetId} not found.");
    }

    public void AddTilesetTexture(int tilesetId, Texture2D texture)
    {
        _tilesetTextures[tilesetId] = texture;
    }
}