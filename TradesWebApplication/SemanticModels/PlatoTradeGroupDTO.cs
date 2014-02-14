using System.Collections.Generic;
using Newtonsoft.Json;

namespace TradesWebApplication.SemanticModels
{
   

    public class PlatoTradeGroupDTO : PlatoTradeGraphNodeDTO
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
        [JsonProperty("http://data.emii.com/ontologies/bcatrading/tradeLineGroupType")]
        public GenericIdPlatoSemanticDTO tradelineGroupType { get; set; }
        [JsonProperty("http://data.emii.com/ontologies/core/canonicalLabel")]
        public CanonicalLabelPlatoDTO canonicalLabel { get; set; }

    }
}