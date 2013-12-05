using System;

namespace TradesWebApplication.Models
{
    public class TradePerformance
    {
        public int TradePerformanceId { get; set; }
        public int? TradeId { get; set; }
        public int? MeasureTypeId { get; set; }
        public string ReturnAplFunction { get; set; }
        public int? ReturnCurrencyId { get; set; }
        public int? ReturnBenchmarkId { get; set; }
        public string ReturnValue { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }

        public virtual Benchmark Benchmark { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual MeasureType Measure_Type { get; set; }
        public virtual Trade Trade { get; set; }
    }
}
