using MonoTiled.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoTiled;

public class Tile
{
    private Vector2 _origin = Vector2.Zero;
    private float _rotation = 0;
    private readonly ITilesetTextureService _tilesetTextureService;

    public Tile(
        int tileSetId,
        int localTileId,
        Rectangle tileSourceRectangle,
        int positionX,
        int positionY,
        ITilesetTextureService tilesetTextureService
        )
    {
        TileSetId = tileSetId;
        LocalTileId = localTileId;
        TileSourceRectangle = tileSourceRectangle;
        PositionX = positionX;
        PositionY = positionY;
        _tilesetTextureService = tilesetTextureService;
    }

    public int TileSetId { get; set; }

    /// <summary>
    /// Local ID within the tileset
    /// LocalTileId is the index of the tile within its tileset, starting from 0
    /// </summary>
    public int LocalTileId { get; set; }
    public Rectangle TileSourceRectangle { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }

    public Color[,] GetColourData(Texture2D tilesetTexture)
    {
        return tilesetTexture.GetColourData(TileSourceRectangle);
    }

    public Texture2D GetTilesetTexture()
    {
        return _tilesetTextureService.GetTilesetTexture(TileSetId);
    }

    public Matrix Matrix => GetTranformationMatrix();

    private Matrix GetTranformationMatrix()
    {
        return Matrix.CreateTranslation(_origin.X, _origin.Y, 0) *
               Matrix.CreateRotationZ(_rotation) *
               Matrix.CreateScale(1f) *
               Matrix.CreateTranslation(PositionX, PositionY, 0);
    }
}

public interface ITilemap
{
    List<Tile> GetNearestTiles(Rectangle boundingBox);
    void Draw();
}

public class Tilemap : ITilemap
{
    private int _tileCountX;
    private int _tileCountY;
    private int _tileWidth;
    private int _tileHeight;
    private string _mapTitle;
    private Tile[,] _tiles;
    private ITilemapRenderer _tilemapRenderer;

    public Tilemap(
        int tileCountX,
        int tileCountY,
        int tileWidth,
        int tileHeight,
        string mapTitle,
        Tile[,] tiles,
        ITilemapRenderer tilemapRenderer
    )
    {
        _tileCountX = tileCountX;
        _tileCountY = tileCountY;
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
        _mapTitle = mapTitle;
        _tiles = tiles;
        _tilemapRenderer = tilemapRenderer;
    }

    // Get nearest tiles to a bounding box (e.g., a sprite's bounding box)
    // This can be used for various purposes, such as rendering or collision detection
    // Returns a list of Tile objects are near the bounding box
    // Overlapping tiles are returned along with tiles that are the bounding box edges are nearest to
    // The method clamps the tile indices to ensure they are within the bounds of the tilemap array
    public List<Tile> GetNearestTiles(Rectangle boundingBox)
    {
        int yLength = _tiles.GetLength(0) - 1;
        int xLength = _tiles.GetLength(1);

        // Calculate tile indices for the bounding box
        // Note: Floor and Ceiling are used to include tiles that the edges are nearest to
        // including tiles that the bounding box is exactly on the edge of
        int leftTile = (int)Math.Floor((float)boundingBox.Left / _tileWidth);
        int rightTile = (int)Math.Ceiling((float)boundingBox.Right / _tileWidth);
        int topTile = (int)Math.Floor((float)boundingBox.Top / _tileHeight);
        int bottomTile = (int)Math.Ceiling((float)boundingBox.Bottom / _tileHeight);

        // Clamp tile indices to be within the bounds of the tilemap array
        // This prevents out-of-bounds errors when accessing the tile array
        // If the bounding box is partially or completely outside the tilemap, the clamping ensures that only valid tile indices are used
        // Example: If the bounding box has Left=-16, Right=16, Top=-16, Bottom=16 and tile size of 16x16
        // the calculated tile indices would be leftTile=-1, rightTile=1, topTile=-1, bottomTile=1
        // After clamping, the tile indices would be leftTile=0, rightTile=1, topTile=0, bottomTile=1
        // This ensures that only tiles (0,0), (1,0), (0,1), and (1,1) are checked for collision
        // which are the only valid tiles in the tilemap
        // Note: xLength and yLength are the maximum valid indices, so we use them directly in Clamp
        // without subtracting 1, because Clamp is inclusive of the maximum value
        // This is different from array indexing where the maximum index is length - 1
        // This is important to avoid off-by-one errors
        leftTile = MathHelper.Clamp(leftTile, 0, xLength);
        rightTile = MathHelper.Clamp(rightTile, 0, xLength);
        topTile = MathHelper.Clamp(topTile, 0, yLength);
        bottomTile = MathHelper.Clamp(bottomTile, 0, yLength);

        List<Tile> nearestTiles = [];

        for (int y = topTile; y <= bottomTile; y++)
        {
            for (int x = leftTile; x <= rightTile - 1; x++)
            {
                if (_tiles[y, x] != null)
                {
                    nearestTiles.Add(_tiles[y, x]);
                }
            }
        }

        return nearestTiles;
    }

    public void Draw()
    {
        _tilemapRenderer.Draw();
    }
}
