using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TiledDotNet.DTOs;

namespace TiledDotNet.Converters
{
    public interface ITiledTilemapJsonConverterService
    {
        TilemapDto GetTilemapDtoFromJsonFile(string tilemapJsonString);
    }

    public class TiledTilemapJsonConverterService : ITiledTilemapJsonConverterService
    {
        public TilemapDto GetTilemapDtoFromJsonFile(string tilemapJsonString)
        {
            var tilemapDTO = JsonConvert.DeserializeObject<TilemapDto>(
            tilemapJsonString,
            new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new TiledTilemapJsonConverter() }
            });

            return tilemapDTO;
        }
    }
    public class TiledTilemapJsonConverter : JsonConverter<TilemapDto>
    {
        public override TilemapDto ReadJson(JsonReader reader,
            Type objectType,
            TilemapDto existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            var tilemapDTO = JsonConvert.DeserializeObject<TilemapDto>(jsonObject.ToString());

            var jObjectTilesets = JsonConvert.DeserializeObject<JArray>(jsonObject["tilesets"].ToString());

            if (jObjectTilesets.Any(x => x["source"] != null))
            {
                throw new NotImplementedException("Tileset source handling is not implemented yet.");
                // TODO: Handle both inline and source tilesets in the same map
                // tilemapDTO.TilesetSourceDTOs = JsonConvert.DeserializeObject<List<TilesetSourceDTO>>(jObjectTilesets.ToString()); 
            }
            else
                tilemapDTO.TilesetDtos = JsonConvert.DeserializeObject<List<TilesetDto>>(jObjectTilesets.ToString());

            return tilemapDTO;
        }

        public override void WriteJson(JsonWriter writer, TilemapDto value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Writing JSON is not implemented for SpritesheetDataConverter");
        }
    }
}
