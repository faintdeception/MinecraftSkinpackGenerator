using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftSkinpackGenerator
{
    public class SkinPack
    {
        public SkinPack(string? localizationName, string? serializeName)
        {
            Skins = new List<Skin>();
            LocalizationName = localizationName;
            SerializeName = serializeName;
        }

        [JsonProperty("localization_name")]
        public string? LocalizationName { get; set; }
        [JsonProperty("serialize_name")]
        public string? SerializeName { get; set; }
        [JsonProperty("skins")]
        public List<Skin> Skins { get; set; }
    }
    public class Skin
    {
        [JsonProperty("localization_name")]
        public string? LocalizationName { get; set; }
        [JsonProperty("geometry")]
        public string? Geometry { get; set; }

        [JsonProperty("texture")]
        public string? Texture { get; set; }
        [JsonProperty("type")]
        public string? Type { get; set; }

        public override string ToString()
        {
            return $"{LocalizationName} {Geometry} {Texture} {Type}";
        }
    }


    public class Manifest
    {
        [JsonProperty("format_version")]
        public int FormatVersion { get; set; }
        [JsonProperty("header")]
        public Header? Header { get; set; }
        [JsonProperty("modules")]        
        public Module[]? Modules { get; set; }
    }

    public class Header
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("uuid")]
        public string? Uuid { get; set; }
        [JsonProperty("version")]
        public int[]? Version { get; set; }
    }

    public class Module
    {
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("uuid")]
        public string? Uuid { get; set; }
        [JsonProperty("version")]
        public int[]? Version { get; set; }
    }


}
