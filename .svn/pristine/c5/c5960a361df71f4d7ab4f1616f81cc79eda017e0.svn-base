using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class TradeLineGroup
    {
        public TradeLineGroup()
        {
            this.TradeLine = new HashSet<TradeLine>();
        }
    
        public int TradeLineGroupId { get; set; }
        public int? TradeLineGroupTypeId { get; set; }
        public string TradeLineGroupUri { get; set; }
        public string TradeLineGroupLabel { get; set; }
        public string TradeLineGroupEditorialLabel { get; set; }

        public virtual ICollection<TradeLine> TradeLine { get; set; }
        public virtual TradeLineGroupType TradeLineGroupType { get; set; }
    }
}
