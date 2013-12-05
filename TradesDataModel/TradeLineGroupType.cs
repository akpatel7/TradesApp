using System.Collections.Generic;

namespace TradesDataModel
{
    public class TradeLineGroupType
    {
        //public TradeLineGroupType()
        //{
        //    this.Trade_Line_Group = new HashSet<TradeLineGroups>();
        //}
    
        public int TradeLineGroupTypeId { get; set; }
        public string TradeLineGroupTypeUri { get; set; }
        public string TradeLineGroupTypeLabel { get; set; }
    
        //public virtual ICollection<TradeLineGroups> Trade_Line_Group { get; set; }
    }
}
