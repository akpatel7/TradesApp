using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class Service
    {
        public Service()
        {
            this.Trades = new HashSet<Trade>();
        }
    
        public int ServiceId { get; set; }
        public string ServiceUri { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceLabel { get; set; }
    
        public virtual ICollection<Trade> Trades { get; set; }
    }
}
