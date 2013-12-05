using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class MeasureType
    {
        public MeasureType()
        {
            this.TradePerformance = new HashSet<TradePerformance>();
        }
    
        public int MeasureTypeId { get; set; }
        public string MeasureTypeLabel { get; set; }
    
        public virtual ICollection<TradePerformance> TradePerformance { get; set; }
    }
}
