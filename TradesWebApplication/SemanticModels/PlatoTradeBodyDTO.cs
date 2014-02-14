using System.Collections.Generic;
using Newtonsoft.Json;

namespace TradesWebApplication.SemanticModels
{
    public class PlatoTradeBodyDTO
    {
        [JsonProperty("@context")]
        public PlatoTradeContextDTO context { get; set; }
        [JsonProperty("@graph")]
        public List<PlatoTradeGraphNodeDTO> graph { get; set; }
    }

    public abstract class PlatoTradeGraphNodeDTO
    {
    }
}