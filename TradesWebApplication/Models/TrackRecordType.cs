using System.Collections.Generic;

namespace TradesWebApplication.Models
{ 
    public class TrackRecordType
    {
        public TrackRecordType()
        {
            this.TrackRecord = new HashSet<TrackRecord>();
        }
    
        public int TrackRecordTypeId { get; set; }
        public string TrackRecordTypeLabel { get; set; }
    
        public virtual ICollection<TrackRecord> TrackRecord { get; set; }
    }
}
