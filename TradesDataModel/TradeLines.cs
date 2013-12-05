using System;

namespace TradesDataModel
{ 
    public class TradeLines
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
    
        //public virtual Positions Position { get; set; }
        //public virtual TradableThings Tradable_Thing { get; set; }
        //public virtual Trades Trade { get; set; }
        //public virtual Trade_Line_Group Trade_Line_Group { get; set; }
    }
}
