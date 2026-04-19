using MonoSprite.Converters;
using MonoSprite.Dtos;
using MonoSprite.Models;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace MonoSprite
{
    public interface IAsepriteSpritesheetService
    {
        AsepriteSpritesheet GetAsepriteSpritesheet(string spritesheetJsonFilePath);
    }

    public class AsepriteSpritesheetService : IAsepriteSpritesheetService
    {
        private IFileSystem _fileSystem;
        private IAsepriteSpritesheetJsonConverterService _asepriteSpritesheetJsonConverterService;

        public AsepriteSpritesheetService(IFileSystem fileSystem,
            IAsepriteSpritesheetJsonConverterService asepriteSpritesheetJsonConverterService)
        {
            _fileSystem = fileSystem;
            _asepriteSpritesheetJsonConverterService = asepriteSpritesheetJsonConverterService;
        }

        public AsepriteSpritesheet GetAsepriteSpritesheet(string spritesheetJsonFilePath)
        {
            // Construct spritesheet steps
            // - Get SpritesheetDTO from .json file
            // - Convert dto to model
            // - Get animations
            //      - Group frames by animation name
            //      

            if (!_fileSystem.File.Exists(spritesheetJsonFilePath))
                throw new FileNotFoundException($"{spritesheetJsonFilePath} not found");

            var spritesheetDto = _asepriteSpritesheetJsonConverterService.MapSpritesheetJsonFileToSpritesheetDto(_fileSystem.File.ReadAllText(spritesheetJsonFilePath));

            var animations = spritesheetDto.FrameDtos
                .GroupBy(x => GetAnimationNameFromFilename(x.Filename))
                .Select(g => new AsepriteAnimation
                {
                    Name = g.Key,
                    Loop = GetLoopFromFilename(g.ToArray()[0].Filename),
                    Frames = g.ToArray().Select(frame => new AsepriteFrame
                    {
                        FrameNumber = (spritesheetDto.AnimationType == AnimationType.Layers) ? GetFrameNumberFromFilename(frame.Filename) : GetTagFrameNumberFromFilename(frame.Filename),
                        Rotated = frame.Rotated,
                        Trimmed = frame.Trimmed,
                        Duration = frame.Duration,
                        SourceRectangle = new SourceRectangle
                        {
                            X = frame.SourceRectangle.X,
                            Y = frame.SourceRectangle.Y,
                            Width = frame.SourceRectangle.Width,
                            Height = frame.SourceRectangle.Height
                        },
                        FrameData = (spritesheetDto.AnimationType == AnimationType.Layers) ? GetFrameDataFromLayersForLayerAnimation(spritesheetDto.LayerDtos, g.Key, GetFrameNumberFromFilename(frame.Filename)) : GetFrameDataFromLayersForTagAnimation(spritesheetDto.LayerDtos, GetFrameNumberFromFilename(frame.Filename))
                    })
                });

            var asepriteSpritesheet = new AsepriteSpritesheet(spritesheetDto.ImageName,
                _fileSystem.Path.GetFileNameWithoutExtension(spritesheetDto.ImageName),
                animations);

            return asepriteSpritesheet;
        }

        private string GetAnimationNameFromFilename(string filename)
        {
            var filenameParts = filename.Split('|');

            var part1Parts = filenameParts[0].Split(',');
            if (part1Parts.Length == 2)
            {
                return part1Parts[0];
            }
            else
            {
                return filenameParts[0];
            }
        }

        private bool GetLoopFromFilename(string filename)
        {
            bool loop = false;

            var filenameParts = filename.Split('|');

            var part1Parts = filenameParts[0].Split(',');
            if (part1Parts.Length == 2)
            {
                loop = part1Parts[1] == nameof(loop) ? true : false;
            }

            return loop;
        }

        private int GetFrameNumberFromFilename(string filename)
        {
            var filenameParts = filename.Split('|');

            int.TryParse(filenameParts[1], out int frameNumber);
            return frameNumber;
        }

        private int GetTagFrameNumberFromFilename(string filename)
        {
            var filenameParts = filename.Split('|');

            if (filenameParts.Length == 3)
            {
                int.TryParse(filenameParts[2], out int tagFrameNumber);
                return tagFrameNumber;
            }
            else
                return 0;
        }

        private string GetFrameDataFromLayersForLayerAnimation(IEnumerable<LayerDto> layerDtos, string animationName, int frameNumber)
        {
            var layersWithCelData = layerDtos.Where(layerDTO => GetLayerName(layerDTO.Name) == animationName
                                          && layerDTO.CelDtos?.Where(celDTO => celDTO.Frame == frameNumber).Count() > 0);

            if (!layersWithCelData.Any())
                return null;

            return layersWithCelData.FirstOrDefault().CelDtos.FirstOrDefault().Data;
        }

        private string GetFrameDataFromLayersForTagAnimation(IEnumerable<LayerDto> layerDtos, int frameNumber)
        {
            var layersWithCelData = layerDtos.Where(x => x.CelDtos?.Where(celDTO => celDTO.Frame == frameNumber).Count() > 0);

            if (!layersWithCelData.Any())
                return null;

            return layersWithCelData.FirstOrDefault().CelDtos.FirstOrDefault().Data;
        }

        private string GetLayerName(string fileName)
        {
            var filenameParts = fileName.Split(',');

            return filenameParts[0];
        }
    }
}
