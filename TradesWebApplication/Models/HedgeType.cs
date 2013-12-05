using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public partial class HedgeType
    {
        public HedgeType()
        {
            this.Trade_Instruction = new HashSet<TradeInstruction>();
        }
    
        public int HedgeId { get; set; }
        public string HedgeLabel { get; set; }
    
        public virtual ICollection<TradeInstruction> Trade_Instruction { get; set; }
    }
}
