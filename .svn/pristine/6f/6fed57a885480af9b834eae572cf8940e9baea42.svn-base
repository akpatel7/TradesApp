using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.ViewModels
{
    public class TradesCreationViewModel
    {
        public IEnumerable<Trade> Trades { get; set; }
        public IEnumerable<Trade_Line_Group> TradeLineGroups { get; set; }
        public IEnumerable<Trade_Line> TradeLines { get; set; }

        public Benchmark Benchmark { get; set; }
        public Currency Currency { get; set; }
        public Length_Type LengthType { get; set; }     
        public Relativity Relativity { get; set; }
        public Service Service { get; set; }
        public Status Status { get; set; }
        public Structure_Type StructureType { get; set; }

        public Trade_Instruction TradeInstruction { get; set; }
        public Hedge_Type HedgeInstruction{ get; set; }
        public IEnumerable<Track_Record> TrackRecord { get; set; }
        public IEnumerable<Trade_Comment> TradeComment { get; set; }
        
        public IEnumerable<Trade_Performance> TradePerformance { get; set; }

        public IEnumerable<Related_Trade> RelatedTrades { get; set; }

    }
}