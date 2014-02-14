using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TradesWebApplication.SemanticModels
{

    public class CanonicalLabelPlatoDTO
    {
        [JsonProperty("@language")]
        public string @language { get; set; }
        [JsonProperty("@value")]
        public string @value { get; set; }
    }

    public class GenericIdPlatoSemanticDTO
    {
        [JsonProperty("@id")]
        public string @id { get; set; }
    }
}