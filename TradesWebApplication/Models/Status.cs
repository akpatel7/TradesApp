using System.Collections.Generic;

namespace TradesWebApplication.Models
{
   
    public class Status
    {
        public Status()
        {
            this.Trades = new HashSet<Trade>();
        }
    
        public int StatusId { get; set; }
        public string StatusLabel { get; set; }
    
        public virtual ICollection<Trade> Trades { get; set; }
    }
}
