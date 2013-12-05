using System;

namespace TradesDataModel
{
    
    public class TradableThings
    {
        //public TradableThings()
        //{
        //    this.Trade_Line = new HashSet<Trade_Line>();
        //}
    
        public int TradableThingId { get; set; }
        public string TradableThingUri { get; set; }
        public Nullable<int> TradableThingClassId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string TradableThingCode { get; set; }
        public string TradableThingLabel { get; set; }
    
        //public virtual Locations Location { get; set; }
        //public virtual Tradable_Thing_Class Tradable_Thing_Class { get; set; }
        //public virtual ICollection<Trade_Line> Trade_Line { get; set; }
    }
}
