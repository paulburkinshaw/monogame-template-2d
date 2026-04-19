using MonoSprite.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoSprite.Converters
{
    public interface IAsepriteSpritesheetJsonConverterService
    {
        SpritesheetDto MapSpritesheetJsonFileToSpritesheetDto(string spritesheetJsonString);
    }

    public class AsepriteSpritesheetJsonConverterService : IAsepriteSpritesheetJsonConverterService
    {
        public SpritesheetDto MapSpritesheetJsonFileToSpritesheetDto(string spritesheetJsonString)
        {
            var spritesheetDTO = JsonConvert.DeserializeObject<SpritesheetDto>(
            spritesheetJsonString,
            new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new AsepriteSpritesheetJsonConverter() }
            });

            if (spritesheetDTO.FrameTagDtos?.Count() > 0)
                spritesheetDTO.AnimationType = AnimationType.Tags;
            else
                spritesheetDTO.AnimationType = AnimationType.Layers;

            return spritesheetDTO;
        }
    }

    public class AsepriteSpritesheetJsonConverter : JsonConverter<SpritesheetDto>
    {
        public override SpritesheetDto ReadJson(JsonReader reader,
            Type objectType,
            SpritesheetDto existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            var spritesheetDTO = JsonConvert.DeserializeObject<SpritesheetDto>(jsonObject["meta"].ToString());
            spritesheetDTO.FrameDtos = JsonConvert.DeserializeObject<FrameDto[]>(jsonObject["frames"].ToString());

            return spritesheetDTO;
        }

        public override void WriteJson(JsonWriter writer, SpritesheetDto value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Writing JSON is not implemented for SpritesheetDataConverter");
        }
    }


}
