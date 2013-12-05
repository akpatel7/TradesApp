using System;

namespace TradesWebApplication.Models
{

    public class TrackRecord
    {
        public int TrackRecordId { get; set; }
        public int? TradeId { get; set; }
        public int? TrackRecordTypeId { get; set; }
        public decimal? TrackRecordValue { get; set; }
        public DateTime? TrackRecordDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }

        public virtual TrackRecordType TrackRecordType { get; set; }
        public virtual Trade Trade { get; set; }
    }
}
