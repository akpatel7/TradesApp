using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class InstructionType
    {
        public InstructionType()
        {
            this.TradeInstruction = new HashSet<TradeInstruction>();
        }
    
        public int InstructionTypeId { get; set; }
        public string InstructionTypeLabel { get; set; }
    
        public virtual ICollection<TradeInstruction> TradeInstruction { get; set; }
    }
}
