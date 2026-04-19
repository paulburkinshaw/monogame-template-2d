using Newtonsoft.Json;
using System.Collections.Generic;

namespace MonoSprite.Dtos
{
    public enum AnimationType
    {
        Layers,
        Tags
    }

    public class SpritesheetDto
    {
        public AnimationType AnimationType { get; set; }

        [JsonProperty("image")]
        public string ImageName { get; set; }

        [JsonProperty("size")]
        public ImageSizeDto ImageSize { get; set; }

        [JsonProperty("format")]
        public string ImageFormat { get; set; }

        [JsonProperty("scale")]
        public int ImageScale { get; set; }
  
        [JsonProperty("frames")]
        public IEnumerable<FrameDto> FrameDtos { get; set; }

        [JsonProperty("layers")]
        public IEnumerable<LayerDto> LayerDtos { get; set; }

        [JsonProperty("frameTags")]
        public IEnumerable<FrameTagDto> FrameTagDtos { get; set; }
    }

    public class ImageSizeDto
    {
        [JsonProperty("w")]
        public int Width { get; set; }

        [JsonProperty("h")]
        public int Height { get; set; }
    }

    public class FrameDto
    {
        public string Filename { get; set; }

        [JsonProperty("frame")]
        public SourceRectangleDto SourceRectangle { get; set; }

        public bool Rotated { get; set; }

        public bool Trimmed { get; set; }

        public int Duration { get; set; }
    }

    public class SourceRectangleDto
    {
        public int X { get; set; }
        public int Y { get; set; }

        [JsonProperty("w")]
        public int Width { get; set; }

        [JsonProperty("h")]
        public int Height { get; set; }
    }

    public class LayerDto
    {
        public string Name { get; set; }

        public int Opacity { get; set; }

        public string BlendMode { get; set; }

        [JsonProperty("cels")]
        public IEnumerable<CelDto> CelDtos { get; set; }
    }

    public class CelDto
    {
        public int Frame { get; set; }
        public string Data { get; set; }
    }

    public class FrameTagDto
    {
        public string Name { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public string Direction { get; set; }

        public string Color { get; set; }
    }

}
