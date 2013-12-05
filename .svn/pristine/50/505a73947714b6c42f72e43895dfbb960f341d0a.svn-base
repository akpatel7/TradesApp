using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class Currency
    {
        public Currency()
        {
            this.Trades = new HashSet<Trade>();
            this.Trade_Performance = new HashSet<TradePerformance>();
        }
    
        public int CurrencyId { get; set; }
        public string CurrencyUri { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyLabel { get; set; }

        public virtual ICollection<Trade> Trades { get; set; }
        public virtual ICollection<TradePerformance> Trade_Performance { get; set; }
    }
}
