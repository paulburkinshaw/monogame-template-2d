using DistributedGameArchitecture.MonoSprite.Converters;
using DistributedGameArchitecture.MonoSprite.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DistributedGameArchitecture.MonoSprite.Test
{
    public class AsepriteSpritesheetJsonConverterTests
    {

        [SetUp]
        public void Setup()
        {

        }


        [Test]
        public void ReadJson_AnimationsInLayers_ReturnsCorrectSpritesheetDto()
        {
            // See .\TestData\SpriteSheetJsonFile1_LayerAnimations.json for the original JSON data.

            // Arrange

            var spritesheetJsonString = "{ \"frames\": [\r\n   {\r\n    \"filename\": \"Ready,loop|0\",\r\n    \"frame\": { \"x\": 0, \"y\": 0, \"w\": 64, \"h\": 64 },\r\n    \"rotated\": false,\r\n    \"trimmed\": false,\r\n    \"spriteSourceSize\": { \"x\": 0, \"y\": 0, \"w\": 64, \"h\": 64 },\r\n    \"sourceSize\": { \"w\": 64, \"h\": 64 },\r\n    \"duration\": 100\r\n   },\r\n   {\r\n    \"filename\": \"Ready,loop|1\",\r\n    \"frame\": { \"x\": 65, \"y\": 0, \"w\": 64, \"h\": 64 },\r\n    \"rotated\": false,\r\n    \"trimmed\": false,\r\n    \"spriteSourceSize\": { \"x\": 0, \"y\": 0, \"w\": 64, \"h\": 64 },\r\n    \"sourceSize\": { \"w\": 64, \"h\": 64 },\r\n    \"duration\": 100\r\n   },\r\n   {\r\n    \"filename\": \"Attack|0\",\r\n    \"frame\": { \"x\": 0, \"y\": 65, \"w\": 64, \"h\": 64 },\r\n    \"rotated\": false,\r\n    \"trimmed\": false,\r\n    \"spriteSourceSize\": { \"x\": 0, \"y\": 0, \"w\": 64, \"h\": 64 },\r\n    \"sourceSize\": { \"w\": 64, \"h\": 64 },\r\n    \"duration\": 100\r\n   },\r\n   {\r\n    \"filename\": \"Attack|1\",\r\n    \"frame\": { \"x\": 65, \"y\": 65, \"w\": 64, \"h\": 64 },\r\n    \"rotated\": false,\r\n    \"trimmed\": false,\r\n    \"spriteSourceSize\": { \"x\": 0, \"y\": 0, \"w\": 64, \"h\": 64 },\r\n    \"sourceSize\": { \"w\": 64, \"h\": 64 },\r\n    \"duration\": 100\r\n   }\r\n ],\r\n \"meta\": {\r\n  \"app\": \"https://www.aseprite.org/\",\r\n  \"version\": \"1.3.7-x64\",\r\n  \"image\": \"spritesheet1.png\",\r\n  \"format\": \"RGBA8888\",\r\n  \"size\": { \"w\": 389, \"h\": 129 },\r\n  \"scale\": \"1\",\r\n  \"frameTags\": [\r\n  ],\r\n  \"layers\": [\r\n   { \"name\": \"Ready,loop\", \"opacity\": 255, \"blendMode\": \"normal\" },\r\n   { \"name\": \"Attack\", \"opacity\": 255, \"blendMode\": \"normal\", \"cels\": [{ \"frame\": 1, \"data\": \"isattacking\" }] }\r\n  ],\r\n  \"slices\": [\r\n  ]\r\n }\r\n}\r\n";      
            
            var mockJsonObject = JObject.Parse(spritesheetJsonString);

            var expected = new SpritesheetDto
            {
                ImageName = "spritesheet1.png",
                ImageSize = new ImageSizeDto
                {
                    Width = 389,
                    Height = 129
                },
                ImageFormat = "RGBA8888",
                ImageScale = 1,
                FrameDtos = new[] {
                    new FrameDto
                    {
                        Filename = "Ready,loop|0",
                        SourceRectangle = new SourceRectangleDto
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
                    new FrameDto
                    {
                        Filename = "Ready,loop|1",
                        SourceRectangle = new SourceRectangleDto
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
                    new FrameDto
                    {
                        Filename = "Attack|0",
                        SourceRectangle = new SourceRectangleDto
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
                    new FrameDto
                    {
                        Filename = "Attack|1",
                        SourceRectangle = new SourceRectangleDto
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
                LayerDtos = new LayerDto[]
                {
                    new LayerDto
                    {
                        Name = "Ready,loop",
                        Opacity = 255,
                        BlendMode = "normal"
                    },
                    new LayerDto
                    {
                        Name = "Attack",
                        Opacity = 255,
                        BlendMode = "normal",
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

            // ACT
            var result = JsonConvert.DeserializeObject<SpritesheetDto>(mockJsonObject.ToString(),
            new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new AsepriteSpritesheetJsonConverter() }
            });

            // ASSERT
            AreEqualByJson(expected, result);
        }

        [Test]
        public void ReadJson_AnimationsInTags_ReturnsCorrectSpritesheetDto()
        {
            // See .\TestData\SpriteSheetJsonFile2_TagAnimations.json for the original JSON data.

            // Arrange
            //var spritesheetJsonString = "{\r\n  \"frames\": [\r\n    {\r\n      \"filename\": \"Attack2|0\",\r\n      \"frame\": {\r\n        \"x\": 0,\r\n        \"y\": 66,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"rotated\": false,\r\n      \"trimmed\": false,\r\n      \"spriteSourceSize\": {\r\n        \"x\": 0,\r\n        \"y\": 0,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"sourceSize\": {\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"duration\": 100\r\n    },\r\n    {\r\n      \"filename\": \"Attack2|1\",\r\n      \"frame\": {\r\n        \"x\": 101,\r\n        \"y\": 66,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"rotated\": false,\r\n      \"trimmed\": false,\r\n      \"spriteSourceSize\": {\r\n        \"x\": 0,\r\n        \"y\": 0,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"sourceSize\": {\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"duration\": 100\r\n    },\r\n    {\r\n      \"filename\": \"Walk,loop|0\",\r\n      \"frame\": {\r\n        \"x\": 0,\r\n        \"y\": 132,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"rotated\": false,\r\n      \"trimmed\": false,\r\n      \"spriteSourceSize\": {\r\n        \"x\": 0,\r\n        \"y\": 0,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"sourceSize\": {\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"duration\": 100\r\n    },\r\n    {\r\n      \"filename\": \"Walk,loop|1\",\r\n      \"frame\": {\r\n        \"x\": 101,\r\n        \"y\": 132,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"rotated\": false,\r\n      \"trimmed\": false,\r\n      \"spriteSourceSize\": {\r\n        \"x\": 0,\r\n        \"y\": 0,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"sourceSize\": {\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"duration\": 100\r\n    }\r\n  ],\r\n  \"meta\": {\r\n    \"app\": \"https://www.aseprite.org/\",\r\n    \"version\": \"1.3.14.4-x64\",\r\n    \"image\": \"attack1_1-sheet.png\",\r\n    \"format\": \"RGBA8888\",\r\n    \"size\": {\r\n      \"w\": 706,\r\n      \"h\": 197\r\n    },\r\n    \"scale\": \"1\",\r\n    \"frameTags\": [\r\n      {\r\n        \"name\": \"Attack2\",\r\n        \"from\": 0,\r\n        \"to\": 1,\r\n        \"direction\": \"forward\",\r\n        \"color\": \"#000000ff\"\r\n      },\r\n      {\r\n        \"name\": \"Walk,loop\",\r\n        \"from\": 2,\r\n        \"to\": 3,\r\n        \"direction\": \"forward\",\r\n        \"color\": \"#000000ff\"\r\n      }\r\n    ],\r\n    \"layers\": [\r\n      {\r\n        \"name\": \"Layer\",\r\n        \"opacity\": 255,\r\n        \"blendMode\": \"normal\",\r\n        \"cels\": [\r\n          {\r\n            \"frame\": 1,\r\n            \"data\": \"isAttacking\"\r\n          }\r\n        ]\r\n      }\r\n    ],\r\n    \"slices\": [\r\n    ]\r\n  }\r\n}\r\n";

            var spritesheetJsonString = "{\r\n  \"frames\": [\r\n    {\r\n      \"filename\": \"Attack|0|0\",\r\n      \"frame\": {\r\n        \"x\": 0,\r\n        \"y\": 66,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"rotated\": false,\r\n      \"trimmed\": false,\r\n      \"spriteSourceSize\": {\r\n        \"x\": 0,\r\n        \"y\": 0,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"sourceSize\": {\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"duration\": 100\r\n    },\r\n    {\r\n      \"filename\": \"Attack|1|1\",\r\n      \"frame\": {\r\n        \"x\": 101,\r\n        \"y\": 66,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"rotated\": false,\r\n      \"trimmed\": false,\r\n      \"spriteSourceSize\": {\r\n        \"x\": 0,\r\n        \"y\": 0,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"sourceSize\": {\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"duration\": 100\r\n    },\r\n    {\r\n      \"filename\": \"Walk,loop|2|0\",\r\n      \"frame\": {\r\n        \"x\": 0,\r\n        \"y\": 132,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"rotated\": false,\r\n      \"trimmed\": false,\r\n      \"spriteSourceSize\": {\r\n        \"x\": 0,\r\n        \"y\": 0,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"sourceSize\": {\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"duration\": 100\r\n    },\r\n    {\r\n      \"filename\": \"Walk,loop|3|1\",\r\n      \"frame\": {\r\n        \"x\": 101,\r\n        \"y\": 132,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"rotated\": false,\r\n      \"trimmed\": false,\r\n      \"spriteSourceSize\": {\r\n        \"x\": 0,\r\n        \"y\": 0,\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"sourceSize\": {\r\n        \"w\": 100,\r\n        \"h\": 65\r\n      },\r\n      \"duration\": 100\r\n    }\r\n  ],\r\n  \"meta\": {\r\n    \"app\": \"https://www.aseprite.org/\",\r\n    \"version\": \"1.3.14.4-x64\",\r\n    \"image\": \"spritesheet1.png\",\r\n    \"format\": \"RGBA8888\",\r\n    \"size\": {\r\n      \"w\": 706,\r\n      \"h\": 197\r\n    },\r\n    \"scale\": \"1\",\r\n    \"frameTags\": [\r\n      {\r\n        \"name\": \"Attack\",\r\n        \"from\": 0,\r\n        \"to\": 1,\r\n        \"direction\": \"forward\",\r\n        \"color\": \"#000000ff\"\r\n      },\r\n      {\r\n        \"name\": \"Walk,loop\",\r\n        \"from\": 2,\r\n        \"to\": 3,\r\n        \"direction\": \"forward\",\r\n        \"color\": \"#000000ff\"\r\n      }\r\n    ],\r\n    \"layers\": [\r\n      {\r\n        \"name\": \"Layer\",\r\n        \"opacity\": 255,\r\n        \"blendMode\": \"normal\",\r\n        \"cels\": [\r\n          {\r\n            \"frame\": 1,\r\n            \"data\": \"isAttacking\"\r\n          }\r\n        ]\r\n      }\r\n    ],\r\n    \"slices\": [\r\n    ]\r\n  }\r\n}\r\n";

            var mockJsonObject = JObject.Parse(spritesheetJsonString);

            var expected = new SpritesheetDto
            {
                ImageName = "spritesheet1.png",
                ImageSize = new ImageSizeDto
                {
                    Width = 706,
                    Height = 197
                },
                ImageFormat = "RGBA8888",
                ImageScale = 1,
                FrameDtos = new[] {
                    new FrameDto
                    {
                        Filename = "Attack|0|0",
                        SourceRectangle = new SourceRectangleDto
                        {
                            X = 0,
                            Y = 66,
                            Width = 100,
                            Height = 65
                        },
                        Rotated = false,
                        Trimmed = false,
                        Duration = 100
                    },
                    new FrameDto
                    {
                        Filename = "Attack|1|1",
                        SourceRectangle = new SourceRectangleDto
                        {
                            X = 101,
                            Y = 66,
                            Width = 100,
                            Height = 65
                        },
                        Rotated = false,
                        Trimmed = false,
                        Duration = 100
                    },
                    new FrameDto
                    {
                        Filename = "Walk,loop|2|0",
                        SourceRectangle = new SourceRectangleDto
                        {
                            X = 0,
                            Y = 132,
                            Width = 100,
                            Height = 65
                        },
                        Rotated = false,
                        Trimmed = false,
                        Duration = 100
                    },
                    new FrameDto
                    {
                        Filename = "Walk,loop|3|1",
                        SourceRectangle = new SourceRectangleDto
                        {
                            X = 101,
                            Y = 132,
                            Width = 100,
                            Height = 65
                        },
                        Rotated = false,
                        Trimmed = false,
                        Duration = 100
                    }
                },
                LayerDtos = new LayerDto[]
                {
                    new LayerDto
                    {
                        Name = "Layer",
                        Opacity = 255,
                        BlendMode = "normal",
                        CelDtos = new CelDto[]
                        {
                            new CelDto
                            {
                                Frame = 1,
                                Data = "isAttacking"
                            }
                        }
                    }
                },
                FrameTagDtos = new FrameTagDto[]
                {
                    new FrameTagDto
                    {
                        Name = "Attack",
                        From = 0,
                        To = 1,
                        Direction = "forward",
                        Color = "#000000ff"
                    },
                    new FrameTagDto
                    {
                        Name = "Walk,loop",
                        From = 2,
                        To = 3,
                        Direction = "forward",
                        Color = "#000000ff"
                    }
                }
            };

            // ACT
            var result = JsonConvert.DeserializeObject<SpritesheetDto>(mockJsonObject.ToString(),
            new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new AsepriteSpritesheetJsonConverter() }
            });

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