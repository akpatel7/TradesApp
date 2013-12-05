using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradesDataModel
{
    
    public class Trades
    {
        public Trades()
        {
            this.RelatedTrade = new HashSet<RelatedTrades>();
            this.RelatedTrade1 = new HashSet<RelatedTrades>();
            this.TrackRecord = new HashSet<TrackRecords>();
            this.TradeComment = new HashSet<TradeComments>();
            this.TradeInstruction = new HashSet<TradeInstructions>();
            this.TradeLine = new HashSet<TradeLines>();
            this.TradePerformance = new HashSet<TradePerformances>();
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

        public virtual Benchmarks Benchmark { get; set; }
        public virtual Currencies Currency { get; set; }
        public virtual LengthTypes LengthType { get; set; }
        public virtual ICollection<RelatedTrades> RelatedTrade { get; set; }
        public virtual ICollection<RelatedTrades> RelatedTrade1 { get; set; }
        public virtual Relativities Relativity { get; set; }
        public virtual Services Service { get; set; }
        public virtual Statuses Status1 { get; set; }
        public virtual StructureTypes StructureType { get; set; }
        public virtual ICollection<TrackRecords> TrackRecord { get; set; }
        public virtual ICollection<TradeComments> TradeComment { get; set; }
        public virtual ICollection<TradeInstructions> TradeInstruction { get; set; }
        public virtual ICollection<TradeLines> TradeLine { get; set; }
        public virtual ICollection<TradePerformances> TradePerformance { get; set; }
    }
}
