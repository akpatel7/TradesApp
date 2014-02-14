using System.Collections.Generic;
using Newtonsoft.Json;

namespace TradesWebApplication.SemanticModels
{
    public class PlatoTradeLineDTO
    {
        [JsonProperty("@set")]
        public List<PlatoTradeLineSetDTO> @set { get; set; }
    }

    public class PlatoTradeLineSetDTO
    {
        public GenericPlatoSemanticWithTypeDTO tradeLineBenchmark { get; set; }
        public GenericPlatoSemanticWithTypeDTO onTradableThing { get; set; }
        public GenericPlatoSemanticWithTypeDTO tradeLinePosition { get; set; }
        public GenericPlatoSemanticDTO tradeLineGroup { get; set; }
        [JsonProperty("@type")]
        public string @type { get; set; }
        [JsonProperty("@id")]
        public string @id { get; set; }
        public string canonicalLabel { get; set; }

    }
}