using Newtonsoft.Json;
using System.Collections.Generic;

namespace TiledDotNet.DTOs
{
    public class TilemapDto
    {
        [JsonProperty("layers")]
        public List<LayerDto> LayerDtos { get; set; }

        public List<TilesetSourceDto> TilesetSourceDtos { get; set; }

        public List<TilesetDto> TilesetDtos { get; set; }

        [JsonProperty("properties")]
        public List<PropertyDto> PropertyDtos { get; set; }

        /// <summary>
        /// Total horizontal tiles
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Total vertical tiles
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///  The tile width in pixels
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// The tile height in pixels
        /// </summary>
        public int TileHeight { get; set; }
    }

    public class LayerDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// Total horizontal tiles
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Total vertical tiles
        /// </summary>
        public int Height { get; set; }

        public double Opacity { get; set; }

        public double OffsetX { get; set; }

        public double OffsetY { get; set; }

        public double ParallaxX { get; set; }

        public double ParallaxY { get; set; }

        public bool RepeatX { get; set; }

        public bool RepeatY { get; set; }

        public string TransparentColor { get; set; }

        public string TintColor { get; set; }

        public bool Visible { get; set; }

        [JsonProperty("properties")]
        public List<PropertyDto> PropertyDtos { get; set; }

        [JsonProperty("data")]
        public uint[] TileGIDs { get; set; }
    }

    public class TilesetSourceDto
    {
        public int firstgid;

        public string source;
    }

    public class TilesetDto
    {
        public string Name { get; set; }
        public int FirstGID { get; set; }
        public string Image { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TileCount { get; set; }
        public int Columns { get; set; }
    }

    public class PropertyDto
    {
        public string Name;

        public string Type;

        public string PropertyType;

        public object Value;
    }
}
