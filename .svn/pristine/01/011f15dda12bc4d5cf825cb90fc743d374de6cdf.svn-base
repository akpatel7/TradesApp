using System;

namespace TradesWebApplication.Models
{
    
    public class TradeInstruction
    {
        public int TradeInstructionId { get; set; }
        public int? TradeId { get; set; }
        public int? RelativityId { get; set; }
        public int? InstructionTypeId { get; set; }
        public int? HedgeId { get; set; }
        public decimal? InstructionEntry { get; set; }
        public DateTime? InstructionEntryDate { get; set; }
        public decimal? InstructionExit { get; set; }
        public DateTime? InstructionExitDate { get; set; }
        public string InstructionLabel { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }

        public virtual HedgeType Hedge_Type { get; set; }
        public virtual InstructionType Instruction_Type { get; set; }
        public virtual Relativity Relativity { get; set; }
        public virtual Trade Trade { get; set; }
    }
}
