using System;

namespace TradesDataModel
{

    public class TrackRecords
    {
        public int TrackRecordId { get; set; }
        public int? TradeId { get; set; }
        public int? TrackRecordTypeId { get; set; }
        public decimal? TrackRecordValue { get; set; }
        public DateTime? TrackRecordDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
    
        //public virtual Track_Record_Type Track_Record_Type { get; set; }
        //public virtual Trades Trade { get; set; }
    }
}
