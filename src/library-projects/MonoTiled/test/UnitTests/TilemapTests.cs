using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using Newtonsoft.Json;

namespace MonoTiled.Test
{
    public class TilemapTests
    {
        [SetUp]
        public void Setup()
        {

        }

        public static IEnumerable<TestCaseData> BoundingBoxesNoOverlap
        {
            get
            {
                yield return new TestCaseData(new Rectangle(0, 8, 64, 64));
                yield return new TestCaseData(new Rectangle(8, 0, 64, 64));
            }
        }

        public static IEnumerable<TestCaseData> BoundingBoxesOverlaps
        {
            get
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        yield return new TestCaseData(new Rectangle(x, y, 64, 64))
                        .SetName($"BoundingBox_Y{y}_X{x}");
                    }
                }
            }
        }

        [TestCaseSource(nameof(BoundingBoxesOverlaps))]
        public void GetNearestTiles_OneTile_ReturnsCorrectTiles(Rectangle boundingBox)
        {
            // Arrange

            //1,0,0,0,0,0,0,0,0,0,
            //0,0,0,0,0,0,0,0,0,0,
            //0,0,0,0,0,0,0,0,0,0,
            //0,0,0,0,0,0,0,0,0,0,
            //0,0,0,0,0,0,0,0,0,0

            var tilesetTextureServiceMock = new Mock<ITilesetTextureService>();
          //  tilesetTextureServiceMock.Setup(s => s.GetTilesetTexture(It.IsAny<int>())).Returns(new Texture2D(new Mock<GraphicsDevice>().Object, 64, 64));

            var tilemapHeight = 5;
            var tilemapWidth = 10;

            Tile[,] tiles = new Tile[tilemapHeight, tilemapWidth];

            tiles[0, 0] = new Tile(
                tileSetId: 1,
                localTileId: 0,
                tileSourceRectangle: new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = 8,
                    Height = 8,
                },
                positionX: 0,
                positionY: 0,
                tilesetTextureService: tilesetTextureServiceMock.Object
            );

            var tilemap = new Tilemap(tilemapWidth, tilemapHeight, 8, 8, tiles);

            var expected = new List<Tile>
            {
                new Tile(
                tileSetId: 1,
                localTileId: 0,
                tileSourceRectangle: new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = 8,
                    Height = 8,
                },
                positionX: 0,
                positionY: 0,
                tilesetTextureService: tilesetTextureServiceMock.Object
                )
            };

            // Act 
            var result = tilemap.GetNearestTiles(boundingBox);

            // Assert
            Assert.That(result.Count, Is.EqualTo(expected.Count));

            CollectionAssert.AreEquivalent(
                expected.Select(e => new { e.TileSetId, e.LocalTileId, e.PositionX, e.PositionY, e.TileSourceRectangle }),
                result.Select(r => new { r.TileSetId, r.LocalTileId, r.PositionX, r.PositionY, r.TileSourceRectangle })
            );

        }

        [TestCaseSource(nameof(BoundingBoxesNoOverlap))]
        public void GetNearestTiles_OneTile_ReturnsNoTiles(Rectangle boundingBox)
        {
            // Arrange

            //1,0,0,0,0,0,0,0,0,0,
            //0,0,0,0,0,0,0,0,0,0,
            //0,0,0,0,0,0,0,0,0,0,
            //0,0,0,0,0,0,0,0,0,0,
            //0,0,0,0,0,0,0,0,0,0

            var tilesetTextureServiceMock = new Mock<ITilesetTextureService>();
           // tilesetTextureServiceMock.Setup(s => s.GetTilesetTexture(It.IsAny<int>())).Returns(new Texture2D(new Mock<GraphicsDevice>().Object, 64, 64));

            var tilemapHeight = 5;
            var tilemapWidth = 10;

            Tile[,] tiles = new Tile[tilemapHeight, tilemapWidth];

            tiles[0, 0] = new Tile(
                tileSetId: 1,
                localTileId: 0,
                tileSourceRectangle: new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = 8,
                    Height = 8,
                },
                positionX: 0,
                positionY: 0,
                tilesetTextureService: tilesetTextureServiceMock.Object
                );

            var tilemap = new Tilemap(tilemapWidth, tilemapHeight, 8, 8, tiles);

            var expected = new List<Tile>();

            // Act 
            var result = tilemap.GetNearestTiles(boundingBox);

            // Assert
            Assert.That(result.Count, Is.EqualTo(expected.Count));

            CollectionAssert.AreEquivalent(
                expected.Select(e => new { e.TileSetId, e.LocalTileId, e.PositionX, e.PositionY, e.TileSourceRectangle }),
                result.Select(r => new { r.TileSetId, r.LocalTileId, r.PositionX, r.PositionY, r.TileSourceRectangle })
            );

        }

        public static IEnumerable<TestCaseData> BoundingBoxe_TwoTiles_Overlapping
        {
            get
            {
                yield return new TestCaseData(new Rectangle(0, 32, 16, 16));
            }
        }

        public static IEnumerable<TestCaseData> BoundingBoxe_TwoTiles_Nearest
        {
            get
            {
                // Just above the two tiles
                // Tiles are at (0,32) and (8,32)
                // Bounding box is at (0,24) with height of 8, so it ends at y=32
                // This should return both tiles since they are the nearest tiles to the bounding box
                yield return new TestCaseData(new Rectangle(0, 24, 8, 8));
            }
        }

        [TestCaseSource(nameof(BoundingBoxe_TwoTiles_Nearest))]
        [TestCaseSource(nameof(BoundingBoxe_TwoTiles_Overlapping))]
        public void GetNearestTiles_TwoTiles_ReturnsCorrectTiles(Rectangle boundingBox)
        {
            // Arrange

            //0,0,0,0,0,0,0,0,0,0, <- the tile is 8 pixels high so on the first row the y axis goes from 0 to 7
            //0,0,0,0,0,0,0,0,0,0, <- the tile is 8 pixels high so on the second row the y axis goes from 8 to 15
            //0,0,0,0,0,0,0,0,0,0, <- the tile is 8 pixels high so on the third row the y axis goes from 16 to 23
            //0,0,0,0,0,0,0,0,0,0, <- the tile is 8 pixels high so on the fourth row the y axis goes from 24 to 31
            //1,1,0,0,0,0,0,0,0,0  <- the tile is 8 pixels high so on the fifth row the y axis goes from 32 to 39

            var tilesetTextureServiceMock = new Mock<ITilesetTextureService>();
            //tilesetTextureServiceMock.Setup(s => s.GetTilesetTexture(It.IsAny<int>())).Returns(new Texture2D());

            var tilemapHeight = 5;
            var tilemapWidth = 10;

            Tile[,] tiles = new Tile[tilemapHeight, tilemapWidth];

            tiles[4, 0] = new Tile(
                tileSetId: 1,
                localTileId: 0,
                tileSourceRectangle: new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = 8,
                    Height = 8,
                },
                positionX: 0,
                positionY: 32,
                tilesetTextureService: tilesetTextureServiceMock.Object
                );

            tiles[4, 1] = new Tile(
                tileSetId: 1,
                localTileId: 1,
                tileSourceRectangle: new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = 8,
                    Height = 8,
                },
                positionX: 8,
                positionY: 32,
                tilesetTextureService: tilesetTextureServiceMock.Object
                );

            var tilemap = new Tilemap(tilemapWidth, tilemapHeight, 8, 8, tiles);

            var expected = new List<Tile>
            {
               new Tile(
                tileSetId: 1,
                localTileId: 0,
                tileSourceRectangle: new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = 8,
                    Height = 8,
                },
                positionX: 0,
                positionY: 32,
                tilesetTextureService: tilesetTextureServiceMock.Object
                ),
                 new Tile(
                tileSetId: 1,
                localTileId: 1,
                tileSourceRectangle: new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = 8,
                    Height = 8,
                },
                positionX: 8,
                positionY: 32,
                tilesetTextureService: tilesetTextureServiceMock.Object
                )
            };

            // Act 
            var result = tilemap.GetNearestTiles(boundingBox);

            // Assert
            Assert.That(result.Count, Is.EqualTo(expected.Count));

            CollectionAssert.AreEquivalent(
                expected.Select(e => new { e.TileSetId, e.LocalTileId, e.PositionX, e.PositionY, e.TileSourceRectangle }),
                result.Select(r => new { r.TileSetId, r.LocalTileId, r.PositionX, r.PositionY, r.TileSourceRectangle })
            );

        }


        #region Helper Methods

        private static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

        #endregion
    }
}