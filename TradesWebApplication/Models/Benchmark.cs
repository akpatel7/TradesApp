using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class Benchmark
    {
        public Benchmark()
        {
            this.Trades = new HashSet<Trade>();
            this.TradePerformance = new HashSet<TradePerformance>();
        }
    
        public int BenchmarkId { get; set; }
        public string BenchmarkUri { get; set; }
        public string BenchmarkCode { get; set; }
        public string BenchmarkLabel { get; set; }
    
        public virtual ICollection<Trade> Trades { get; set; }
        public virtual ICollection<TradePerformance> TradePerformance { get; set; }
    }
}
