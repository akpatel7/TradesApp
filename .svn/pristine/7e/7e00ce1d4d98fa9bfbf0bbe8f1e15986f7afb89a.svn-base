using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class LengthType
    {
        public LengthType()
        {
            this.Trades = new HashSet<Trade>();
        }
    
        public int LengthTypeId { get; set; }
        public string LengthTypeLabel { get; set; }
    
        public virtual ICollection<Trade> Trades { get; set; }
    }
}
