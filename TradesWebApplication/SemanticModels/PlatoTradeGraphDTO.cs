using System.Collections.Generic;
using Newtonsoft.Json;

namespace TradesWebApplication.SemanticModels
{
    public class PlatoTradeGraphDTO
    {
        [JsonProperty("@type")]
        public string @type = "TradeRecommendation";
        public List<PlatoTradeLineDTO> tradeLine { get; set; }
        public GenericIdPlatoSemanticDTO rights { get; set; }
        public GenericPlatoSemanticDTO service { get; set; }
        public GenericPlatoSemanticDTO tradeBenchmark { get; set; }
        [JsonProperty("@id")]
        public string @id { get; set; }
        public string canonicalLabel { get; set; }
    }
}