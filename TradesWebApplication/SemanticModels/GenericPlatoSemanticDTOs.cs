using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TradesWebApplication.SemanticModels
{
    public class GenericPlatoSemanticDTO
    {
            [JsonProperty("@type")]
            public string @type { get; set; }
            [JsonProperty("@id")]
            public string @id { get; set; }
            public string canonicalLabel { get; set; }   
    }

    public class GenericPlatoSemanticWithTypeDTO
    {
        public string Type { get; set; }
        [JsonProperty("@type")]
        public string @type { get; set; }
        [JsonProperty("@id")]
        public string @id { get; set; }
        public string canonicalLabel { get; set; }
    }

    public class GenericIdPlatoSemanticDTO
    {
        [JsonProperty("@id")]
        public string @id { get; set; }
    }
}