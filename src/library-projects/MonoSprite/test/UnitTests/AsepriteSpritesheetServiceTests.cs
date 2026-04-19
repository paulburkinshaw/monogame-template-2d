using DistributedGameArchitecture.MonoSprite.Converters;
using DistributedGameArchitecture.MonoSprite.Dtos;
using Moq;
using Newtonsoft.Json;
using System.IO.Abstractions.TestingHelpers;

namespace DistributedGameArchitecture.MonoSprite.Test
{
    public class AsepriteSpritesheetServiceTests
    {


        [SetUp]
        public void Setup()
        {

        }


        [Test]
        public void GetAsepriteSpritesheet_AnimationsInLayers_ReturnsCorrectAsepriteSpritesheet()
        {
            // Arrange         
            var mockSpritesheetJsonString = "mockSpritesheetJsonString";

            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"\Content\Spritesheets\Spritesheet1.json", new MockFileData(mockSpritesheetJsonString) }
            });

            var mockSpritesheetJsonFilePath = @"\Content\Spritesheets\Spritesheet1.json";

            var asepriteSpritesheetJsonConverterServiceMock = new Mock<IAsepriteSpritesheetJsonConverterService>();

            var spritesheetDTO = new SpritesheetDto
            {
                ImageName = "Image1.png",
                FrameDtos = new[]
                {
                    new FrameDto
                    {
                        Filename = "Ready,loop|0",
                        SourceRectangle = new SourceRectangleDto
                        {
                            X = 0,
                            Y = 0,
                            Width = 64,
                            Height = 64
                        }
                    },
                    new FrameDto
                    {
                        Filename = "Ready,loop|1",
                        SourceRectangle = new SourceRectangleDto
                        {
                            X = 65,
                            Y = 0,
                            Width = 64,
                            Height = 64
                        }
                    },
                    new FrameDto
                    {
                        Filename = "Attack|0",
                        SourceRectangle = new SourceRectangleDto
                        {
                            X = 0,
                            Y = 65,
                            Width = 64,
                            Height = 64
                        }
                    },
                    new FrameDto
                    {
                        Filename = "Attack|1",
                        SourceRectangle = new SourceRectangleDto
                        {
                            X = 65,
                            Y = 65,
                            Width = 64,
                            Height = 64
                        }
                    }
                },
                LayerDtos = new[]
                {
                    new LayerDto
                    {
                        Name = "Ready,loop",
                        BlendMode = "nomal",
                        Opacity = 255,
                        CelDtos = null
                    },
                    new LayerDto
                    {
                        Name = "Attack",
                        BlendMode = "nomal",
                        Opacity = 255,
                        CelDtos = new CelDto[]
                        {
                            new CelDto
                            {
                                Frame = 1,
                                Data = "isattacking"
                            }
                        }
                    }
                },
                FrameTagDtos = new FrameTagDto[]
                {
                    // No frame tags in this test case
                }
            };

            asepriteSpritesheetJsonConverterServiceMock
                .Setup(x => x.MapSpritesheetJsonFileToSpritesheetDTO(It.Is<string>(spritesheetJsonString => spritesheetJsonString == mockSpritesheetJsonString)))
                .Returns(spritesheetDTO);

            var expected = new
            {
                SpritesheetImageName = "Image1",
                SpritesheetImageFileName = "Image1.png",
                Animations = new AsepriteAnimation[]
                {
                    new AsepriteAnimation
                    {
                        Name = "Ready",
                        Loop = true,
                        Frames = new AsepriteFrame[]
                        {
                            new AsepriteFrame
                            {
                                FrameNumber = 0,
                                SourceRectangle = new SourceRectangle
                                {
                                    X = 0,
                                    Y = 0,
                                    Width = 64,
                                    Height = 64
                                },
                                Rotated = false,
                                Trimmed = false,
                                Duration = 0,
                                FrameData = null
                            },
                            new AsepriteFrame
                            {
                                FrameNumber = 1,
                                SourceRectangle = new SourceRectangle
                                {
                                    X = 65,
                                    Y = 0,
                                    Width = 64,
                                    Height = 64
                                },
                                Rotated = false,
                                Trimmed = false,
                                Duration = 0,
                                FrameData = null
                            }
                        }
                    },
                    new AsepriteAnimation
                    {
                        Name = "Attack",
                        Loop = false,
                        Frames = new AsepriteFrame[]
                        {
                            new AsepriteFrame
                            {
                                FrameNumber = 0,
                                SourceRectangle = new SourceRectangle
                                {
                                    X = 0,
                                    Y = 65,
                                    Width = 64,
                                    Height = 64
                                },
                                Rotated = false,
                                Trimmed = false,
                                Duration = 0,
                                FrameData = null
                            },
                            new AsepriteFrame
                            {
                                FrameNumber = 1,
                                SourceRectangle = new SourceRectangle
                                {
                                    X = 65,
                                    Y = 65,
                                    Width = 64,
                                    Height = 64
                                },
                                Rotated = false,
                                Trimmed = false,
                                Duration = 0,
                                FrameData = "isattacking"
                            }
                        }
                    }
                }
            };

            // ACT
            var aepriteSpritesheetService = new AsepriteSpritesheetService(mockFileSystem, asepriteSpritesheetJsonConverterServiceMock.Object);

            var result = aepriteSpritesheetService.GetAsepriteSpritesheet(mockSpritesheetJsonFilePath);

            // ASSERT
            AreEqualByJson(expected, result);
        }

        [Test]
        public void GetAsepriteSpritesheet_AnimationsInTags_ReturnsCorrectAsepriteSpritesheet()
        {
            // Arrange         
            var mockSpritesheetJsonString = "mockSpritesheetJsonString";

            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"\Content\Spritesheets\Spritesheet1.json", new MockFileData(mockSpritesheetJsonString) }
            });

            var mockSpritesheetJsonFilePath = @"\Content\Spritesheets\Spritesheet1.json";

            var asepriteSpritesheetJsonConverterServiceMock = new Mock<IAsepriteSpritesheetJsonConverterService>();

            var spritesheetDTO = new SpritesheetDTO
            {
                AnimationType = AnimationType.Tags,
                ImageName = "Image1.png",
                ImageSize = new ImageSizeDTO
                {
                    Width = 706,
                    Height = 197
                },
                ImageFormat = "RGBA8888",
                ImageScale = 1,
                FrameDTOs = new[] {
                    new FrameDTO
                    {
                        Filename = "Attack|0|0",
                        SourceRectangle = new SourceRectangleDTO
                        {
                            X = 0,
                            Y = 0,
                            Width = 64,
                            Height = 64
                        },
                        Rotated = false,
                        Trimmed = false,
                        Duration = 100
                    },
                    new FrameDTO
                    {
                        Filename = "Attack|1|1",
                        SourceRectangle = new SourceRectangleDTO
                        {
                            X = 65,
                            Y = 0,
                            Width = 64,
                            Height = 64
                        },
                        Rotated = false,
                        Trimmed = false,
                        Duration = 100
                    },
                    new FrameDTO
                    {
                        Filename = "Walk,loop|2|0",
                        SourceRectangle = new SourceRectangleDTO
                        {
                            X = 0,
                            Y = 65,
                            Width = 64,
                            Height = 64
                        },
                        Rotated = false,
                        Trimmed = false,
                        Duration = 100
                    },
                    new FrameDTO
                    {
                        Filename = "Walk,loop|3|1",
                        SourceRectangle = new SourceRectangleDTO
                        {
                            X = 65,
                            Y = 65,
                            Width = 64,
                            Height = 64
                        },
                        Rotated = false,
                        Trimmed = false,
                        Duration = 100
                    }
                },
                LayerDTOs = new LayerDTO[]
                {
                    new LayerDTO
                    {
                        Name = "Layer",
                        Opacity = 255,
                        BlendMode = "normal",
                        CelDTOs = new CelDTO[]
                        {
                            new CelDTO
                            {
                                Frame = 1,
                                Data = "isattacking"
                            }
                        }
                    }
                },
                FrameTagDTOs = new FrameTagDTO[]
                {
                    new FrameTagDTO
                    {
                        Name = "Attack",
                        From = 0,
                        To = 1,
                        Direction = "forward",
                        Color = "#000000ff"
                    },
                    new FrameTagDTO
                    {
                        Name = "Walk,loop",
                        From = 2,
                        To = 3,
                        Direction = "forward",
                        Color = "#000000ff"
                    }
                }
            };

            asepriteSpritesheetJsonConverterServiceMock
                .Setup(x => x.MapSpritesheetJsonFileToSpritesheetDTO(It.Is<string>(spritesheetJsonString => spritesheetJsonString == mockSpritesheetJsonString)))
                .Returns(spritesheetDTO);

            var expected = new
            {
                SpritesheetImageName = "Image1",
                SpritesheetImageFileName = "Image1.png",
                Animations = new AsepriteAnimation[]
                {
                    new AsepriteAnimation
                    {
                        Name = "Attack",
                        Loop = false,
                        Frames = new AsepriteFrame[]
                        {
                            new AsepriteFrame
                            {
                                FrameNumber = 0,
                                SourceRectangle = new SourceRectangle
                                {
                                    X = 0,
                                    Y = 0,
                                    Width = 64,
                                    Height = 64
                                },
                                Rotated = false,
                                Trimmed = false,
                                Duration = 100,
                                FrameData = null
                            },
                            new AsepriteFrame
                            {
                                FrameNumber = 1,
                                SourceRectangle = new SourceRectangle
                                {
                                    X = 65,
                                    Y = 0,
                                    Width = 64,
                                    Height = 64
                                },
                                Rotated = false,
                                Trimmed = false,
                                Duration = 100,
                                FrameData = "isattacking"
                            }
                        }
                    },
                    new AsepriteAnimation
                    {
                        Name = "Walk",
                        Loop = true,
                        Frames = new AsepriteFrame[]
                        {
                            new AsepriteFrame
                            {
                                FrameNumber = 0,
                                SourceRectangle = new SourceRectangle
                                {
                                    X = 0,
                                    Y = 65,
                                    Width = 64,
                                    Height = 64
                                },
                                Rotated = false,
                                Trimmed = false,
                                Duration = 100,
                                FrameData = null
                            },
                            new AsepriteFrame
                            {
                                FrameNumber = 1,
                                SourceRectangle = new SourceRectangle
                                {
                                    X = 65,
                                    Y = 65,
                                    Width = 64,
                                    Height = 64
                                },
                                Rotated = false,
                                Trimmed = false,
                                Duration = 100,
                                FrameData = null
                            }
                        }
                    }
                }
            };

            // ACT
            var aepriteSpritesheetService = new AsepriteSpritesheetService(mockFileSystem, asepriteSpritesheetJsonConverterServiceMock.Object);

            var result = aepriteSpritesheetService.GetAsepriteSpritesheet(mockSpritesheetJsonFilePath);

            // ASSERT
            AreEqualByJson(expected, result);
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