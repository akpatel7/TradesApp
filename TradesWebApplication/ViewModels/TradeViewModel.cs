using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradesWebApplication.Models;

namespace TradesWebApplication.ViewModels
{
    public class TradeViewModel : ViewInformation
    {
        // Trade
        public Trade Trade;
        public List<Trade> Trades;
        public int TradeId { get; set; }

        // Service
        public int ServiceId { get; set; }
        public List<Service> Services { get; set; }

        // Trade Type
        public int TradeTypeId { get; set; }
        public List<LengthType> TradeTypes { get; set; }

        // Benchmark
        public int BenchmarkTypeId { get; set; }
        public List<Relativity> BenchmarkType { get; set; }

        // Benchmark selection
        // TODO: Typeahead Field
        public int BenchmarkId { get; set; }
        public List<Benchmark> Benchmarks { get; set; }

        // canonical label
        public string CanonicalLabel { get; set; }

        // editorial label
        public string EditorialLabel { get; set; }

        // update time - caluclated - hidden
        public string CreatedOn { get; set; }

        // update time - caluclated - hidden
        public string LastUpdated { get; set; }

        //Status
        public int? StatusId { get; set; }

        ///***Groups
        // trade structure
        public int StructureTypeId { get; set; }
        public List<StructureType> StructureTypes { get; set; }
        //
        // trade groups
        public List<TradeLineGroup> TradeGroups { get; set; }
        //
        // trade line items
        public List<TradeLine> TradeLines { get; set; }
        ///***

        ///***Instructions
        // entry level
        public decimal InstructionEntry { get; set; }
        // start date
        public string InstructionEntryDate { get; set; }
        // exit level
        public decimal? InstructionExit { get; set; }
        // exit date
        public string InstructionExitDate { get; set; }
        // instruction
        public int? InstructionTypeId { get; set; }
        public List<InstructionType> InstructionTypes { get; set; }
        public string InstructionLabel { get; set; }
        // hedge instruction
        public int? HedgeId { get; set; }
        public List<HedgeType> HedgeInsructionTypes { get; set; }
        // currency
        // TODO: Typeahead Field
        public int? CurrencyId { get; set; }
        public List<Currency> TradeExecutionCurrencies { get; set; }
        ///***

        ///*** supplementary info
        // linked trades
        // TODO: Typeahead Field
        public int? RelatedTradeId { get; set; }
        public List<Trade> ReleatedTrades { get; set; }
        // APL function
        public string AplFunction { get; set; }
        ///

        //***FX Spot and Carry
        // mark to mark rate
        public string MarkToMarketRate { get; set; }
        // internset rate diff
        public string InterestRateDifferential { get; set; }
        //***

        ///*** absolute performance
        public List<MeasureType> MeasureTypes { get; set; }
        public int? AbsoluteMeasureTypeId { get; set; }
        public int? AbsoluteCurrencyId { get; set; }
        // return value
        public string AbsoluteReturnValue { get; set; }
        ///***

        ///*** rel performance
        public int? RelativeMeasureTypeId { get; set; }
        // TODO: Typeahead Field
        public int? RelativeCurrencyId { get; set; }
        // TODO: Typeahead Field
        public int? ReturnBenchmarkId { get; set; }
        // return value
        public string RelativeReturnValue { get; set; }
        ///***

        //*** comments
        public string TradeComments { get; set; }
    }
}