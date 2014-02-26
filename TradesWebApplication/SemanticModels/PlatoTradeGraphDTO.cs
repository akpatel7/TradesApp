using System.Collections.Generic;
using Newtonsoft.Json;

namespace TradesWebApplication.SemanticModels
{



    public class PlatoTradeGraphSummary : PlatoTradeGraphNodeDTO
    {
        [JsonProperty("@id")] //trade id
        public string @id { get; set; }

        [JsonProperty("@type")] public string @type = @"http://data.emii.com/ontologies/bca/TradeRecommendation";
        //deletestatus
        [JsonProperty("http://data.emii.com/ontologies/bca/informedByView")]
        public GenericIdPlatoSemanticDTO informedByView { get; set; }

        //service
        [JsonProperty("http://data.emii.com/ontologies/bca/service")]
        public GenericIdPlatoSemanticDTO service { get; set; }

        //benchmark
        [JsonProperty("http://data.emii.com/ontologies/bcatrading/tradeBenchmark")]
        public GenericIdPlatoSemanticDTO benchmark { get; set; }

        //canionicalLabel for trade
        [JsonProperty("http://data.emii.com/ontologies/core/canonicalLabel")]
        public CanonicalLabelPlatoDTO canonicalLabel { get; set; }

        //enumerate flakeids of tradelines
        [JsonProperty("http://data.emii.com/ontologies/bcatrading/tradeLine")]
        public List<GenericIdPlatoSemanticDTO> tradeLine { get; set; }

        //status
        [JsonProperty("http://data.emii.com/ontologies/core/status")]
        public GenericIdPlatoSemanticDTO status { get; set; }

        [JsonProperty("http://purl.org/dc/elements/1.1/rights")]
        public GenericIdPlatoSemanticDTO rights { get; set; }

        public PlatoTradeGraphSummary()
        {
            informedByView = new GenericIdPlatoSemanticDTO(){ id = @"http://data.emii.com/ontologies/bca/informedByView" }

    ;
            service = new GenericIdPlatoSemanticDTO();
            benchmark = new GenericIdPlatoSemanticDTO();
            canonicalLabel = new CanonicalLabelPlatoDTO();
            tradeLine = new List<GenericIdPlatoSemanticDTO>();
            status = new GenericIdPlatoSemanticDTO();
            rights = new GenericIdPlatoSemanticDTO();

        }
    }

}