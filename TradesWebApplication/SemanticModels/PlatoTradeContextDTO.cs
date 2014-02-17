using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TradesWebApplication.SemanticModels
{
    public class PlatoTradeContextDTO
    {
        [JsonProperty ("@language")]
        public string language { get; set; }
        public string rights { get; set; }
        public string TradeRecommendation { get; set; }
        public string informedByView { get; set; }
        public string Service { get; set; }
        public string TradeLine { get; set; }
        public string TradeLineGroup { get; set; }
        public string Type { get; set; }
        public string canonicalLabel { get; set; }
        public string tradeLine { get; set; }
        public string tradeLineGroup { get; set; }
        public string tradeBenchmark { get; set; }
        public string onTradableThing { get; set; }
        public string service { get; set; }
        public string tradeLinePosition { get; set; }

        public PlatoTradeContextDTO()
        {
            language = "en";
            rights = @"http://purl.org/dc/elements/1.1/rights";
            TradeRecommendation = @"http://data.emii.com/ontologies/bca/TradeRecommendation";
            informedByView = @"http://data.emii.com/ontologies/bca/informedByView";
            Service = @"http://data.emii.com/ontologies/bca/Service";
            tradeBenchmark = @"http://data.emii.com/ontologies/bcatrading/tradeBenchmark";
            TradeLine = @"http://data.emii.com/ontologies/bcatrading/TradeLine";
            TradeLineGroup = @"http://data.emii.com/ontologies/bcatrading/TradeLineGroup";
            Type = @"http://www.w3.org/1999/02/22-rdf-syntax-ns#Type";
            canonicalLabel = @"http://data.emii.com/ontologies/core/canonicalLabel";
            tradeLine = @"http://data.emii.com/ontologies/bcatrading/tradeLine";
            tradeLineGroup = @"http://data.emii.com/ontologies/bcatrading/tradeLineGroup";
            tradeBenchmark = @"http://data.emii.com/ontologies/bcatrading/tradeBenchmark";
            onTradableThing = @"http://data.emii.com/ontologies/bcatrading/onTradableThing";
            service = @"http://data.emii.com/ontologies/bca/service";
            tradeLinePosition = @"http://data.emii.com/ontologies/bcatrading/tradeLinePosition";
        }
    }
}