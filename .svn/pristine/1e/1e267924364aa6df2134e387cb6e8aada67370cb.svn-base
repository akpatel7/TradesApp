using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class TradeLineGroupType
    {
        public TradeLineGroupType()
        {
            this.TradeLineGroup = new HashSet<TradeLineGroup>();
        }
    
        public int TradeLineGroupTypeId { get; set; }
        public string TradeLineGroupTypeUri { get; set; }
        public string TradeLineGroupTypeLabel { get; set; }
    
        public virtual ICollection<TradeLineGroup> TradeLineGroup { get; set; }
    }
}
