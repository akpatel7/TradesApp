using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class Position
    {
        public Position()
        {
            this.TradeLine = new HashSet<TradeLine>();
        }
    
        public int PositionId { get; set; }
        public string PositionUri { get; set; }
        public string PositionLabel { get; set; }
        public int? PositionRelativityId { get; set; }

        public virtual Relativity Relativity { get; set; }
        public virtual ICollection<TradeLine> TradeLine { get; set; }
    }
}
