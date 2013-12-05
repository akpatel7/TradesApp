using System;

namespace TradesWebApplication.Models
{ 
    public class TradeLine
    {
        public int TradeLineId { get; set; }
        public string TradeLineUri { get; set; }
        public int? TradeId { get; set; }
        public int? TradeLineGroupId { get; set; }
        public int? TradableThingId { get; set; }
        public int? PositionId { get; set; }
        public string TradeLineLabel { get; set; }
        public string TradeLineEditorialLabel { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }

        public virtual Position Position { get; set; }
        public virtual TradableThing Tradable_Thing { get; set; }
        public virtual Trade Trade { get; set; }
        public virtual TradeLineGroup Trade_Line_Group { get; set; }
    }
}
