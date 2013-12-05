using System;
using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    
    public class TradableThing
    {
        public TradableThing()
        {
            this.TradeLine = new HashSet<TradeLine>();
        }
    
        public int TradableThingId { get; set; }
        public string TradableThingUri { get; set; }
        public Nullable<int> TradableThingClassId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string TradableThingCode { get; set; }
        public string TradableThingLabel { get; set; }

        public virtual Location Location { get; set; }
        public virtual TradableThingClass TradableThingClass { get; set; }
        public virtual ICollection<TradeLine> TradeLine { get; set; }
    }
}
