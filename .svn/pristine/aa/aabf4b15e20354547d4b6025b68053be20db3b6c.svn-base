using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class Location
    {
        public Location()
        {
            this.TradableThing = new HashSet<TradableThing>();
        }
    
        public int LocationId { get; set; }
        public string LocationUri { get; set; }
        public string LocationCode { get; set; }
        public string LocationLabel { get; set; }
    
        public virtual ICollection<TradableThing> TradableThing { get; set; }
    }
}
