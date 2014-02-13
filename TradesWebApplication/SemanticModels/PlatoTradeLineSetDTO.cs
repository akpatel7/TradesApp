using System.Collections.Generic;

namespace TradesWebApplication.SemanticModels
{
    public class PlatoTradeLineDTO
    {
        public List<PlatoTradeLineSetDTO> @set { get; set; }
    }

    public class PlatoTradeLineSetDTO
    {
        public GenericPlatoSemanticWithTypeDTO tradeLineBenchmark { get; set; }
        public GenericPlatoSemanticWithTypeDTO onTradableThing { get; set; }
        public GenericPlatoSemanticWithTypeDTO tradeLinePosition { get; set; }
        public GenericPlatoSemanticDTO tradeLineGroup { get; set; }
        public string @type { get; set; }
        public string @id { get; set; }
        public string canonicalLabel { get; set; }

    }
}