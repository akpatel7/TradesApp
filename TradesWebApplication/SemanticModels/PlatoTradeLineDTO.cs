using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TradesWebApplication.SemanticModels
{
    public class PlatoTradeLineDTO : PlatoTradeGraphNodeDTO
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
        [JsonProperty("http://data.emii.com/ontologies/bcatrading/onTradableThing")]
        public GenericIdPlatoSemanticDTO onTradableThing { get; set; }
        [JsonProperty("http://data.emii.com/ontologies/bcatrading/tradeLineGroup")]
        public GenericIdPlatoSemanticDTO tradeLineGroup { get; set; }
        [JsonProperty("http://data.emii.com/ontologies/bcatrading/tradeLinePosition")]
        public GenericIdPlatoSemanticDTO tradeLinePosition { get; set; }
        [JsonProperty("http://data.emii.com/ontologies/core/canonicalLabel")]
        public CanonicalLabelPlatoDTO canonicalLabel { get; set; }

    }
}