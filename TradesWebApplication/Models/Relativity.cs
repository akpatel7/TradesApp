using System.Collections.Generic;

namespace TradesWebApplication.Models
{
    public class Relativity
    {
        public Relativity()
        {
            this.Positions = new HashSet<Position>();
            this.Trade_Instruction = new HashSet<TradeInstruction>();
            this.Trades = new HashSet<Trade>();
        }
    
        public int RelativityId { get; set; }
        public string RelativityLabel { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<TradeInstruction> Trade_Instruction { get; set; }
        public virtual ICollection<Trade> Trades { get; set; }
    }
}
