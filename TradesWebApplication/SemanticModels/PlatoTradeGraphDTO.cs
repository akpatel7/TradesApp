using System.Collections.Generic;

namespace TradesWebApplication.SemanticModels
{
    public class PlatoTradeGraphDTO
    {
        public string @type = "TradeRecommendation";
        public List<PlatoTradeLineDTO> tradeLine { get; set; }
        public GenericIdPlatoSemanticDTO rights { get; set; }
        public GenericPlatoSemanticDTO service { get; set; }
        public GenericPlatoSemanticDTO tradeBenchmark { get; set; }
        public string @id { get; set; }
        public string canonicalLabel { get; set; }
    }
}