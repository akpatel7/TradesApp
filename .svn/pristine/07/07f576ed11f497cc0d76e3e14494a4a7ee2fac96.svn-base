using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    
    public partial class StructureType
    {
        public StructureType()
        {
            this.Trades = new HashSet<Trade>();
        }
    
        public int StructureTypeId { get; set; }
        public string StructureTypeLabel { get; set; }
    
        public virtual ICollection<Trade> Trades { get; set; }
    }
}
