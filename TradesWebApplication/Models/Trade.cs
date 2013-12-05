using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    
    public class Trade
    {
        public Trade()
        {
            this.RelatedTrade = new HashSet<RelatedTrade>();
            this.RelatedTrade1 = new HashSet<RelatedTrade>();
            this.TrackRecord = new HashSet<TrackRecord>();
            this.TradeComment = new HashSet<TradeComment>();
            this.TradeInstruction = new HashSet<TradeInstruction>();
            this.TradeLine = new HashSet<TradeLine>();
            this.TradePerformance = new HashSet<TradePerformance>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TradeId { get; set; }

        public string TradeUri { get; set; }
        public int? RelativityId { get; set; }
        public int? LengthTypeId { get; set; }
        public int? StructureTypeId { get; set; }
        public int? ServiceId { get; set; }
        public int? CurrencyId { get; set; }
        public int? BenchmarkId { get; set; }
        public string TradeLabel { get; set; }
        public string TradeEditorialLabel { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? Status { get; set; }

        public virtual Benchmark Benchmark { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual LengthType LengthType { get; set; }
        public virtual ICollection<RelatedTrade> RelatedTrade { get; set; }
        public virtual ICollection<RelatedTrade> RelatedTrade1 { get; set; }
        public virtual Relativity Relativity { get; set; }
        public virtual Service Service { get; set; }
        public virtual Status Status1 { get; set; }
        public virtual StructureType StructureType { get; set; }
        public virtual ICollection<TrackRecord> TrackRecord { get; set; }
        public virtual ICollection<TradeComment> TradeComment { get; set; }
        public virtual ICollection<TradeInstruction> TradeInstruction { get; set; }
        public virtual ICollection<TradeLine> TradeLine { get; set; }
        public virtual ICollection<TradePerformance> TradePerformance { get; set; }
    }
}
