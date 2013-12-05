using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    
    public class TradableThingClass
    {
        public TradableThingClass()
        {
            this.TradableThing = new HashSet<TradableThing>();
        }
    
        public int TradableThingClassId { get; set; }
        public string TradableThingClassUri { get; set; }
        public string TradableThingClassLabel { get; set; }
        public string TradableThingClassEditorialLabel { get; set; }
    
        public virtual ICollection<TradableThing> TradableThing { get; set; }
    }
}
