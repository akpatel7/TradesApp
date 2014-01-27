using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.ViewModels
{
    public class TradesCreationViewModel
    {
        // Trade
        public int trade_id { get; set; }
        public Trade Trade { get; set; }
        public List<TradeLineGroupViewModel> TradeLineGroups { get; set; }
        public List<TradeLineViewModel> TradeLines { get; set; }

        // service
        [Required(ErrorMessage = "This field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please make a selection")]
        [DisplayName("Service:")]
        public int service_id { get; set; }
        public List<Service> Services { get; set; }
        
        // trade type
        [Required(ErrorMessage = "This field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please make a selection")]
        [DisplayName("Trade Type:")]
        public int length_type_id { get; set; }
        public List<Length_Type> LengthTypes { get; set; }
        
        // benchmark
        [Required(ErrorMessage = "This field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please make a selection")]
        [DisplayName("Benchmark:")]
        public int relativity_id { get; set; }
        public List<Relativity> Relativitys { get; set; }
        
        // benchmark selection
        [DisplayName("Benchmark Selection:")]
        //[Range(1, int.MaxValue, ErrorMessage = "Please make a selection")]
        public int? benchmark_id { get; set; }
        public List<Benchmark> Benchmarks { get; set; }
        
        // update time
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("System Created:")]
        public string created_on { get; set; }

        // update time
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Last Updated:")]
        public string last_updated { get; set; }
        
        // canonical label
        [DisplayName("Canonical Label:")]
        public string trade_label { get; set; }
        
        // editorial label
        [DisplayName("Editorial Label:")]
        public string trade_editorial_label { get; set; }
        
        // trade structure
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Trade Structure:")]
        public int structure_type_id { get; set; }
        public List<Structure_Type> StructureTypes { get; set; }

        // status
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("TStatus:")]
        public int? status { get; set; }
        public List<Status> Statuses { get; set; }
        
        //------------------------------------
        // entry level
        [Required(ErrorMessage = "This field is required.")]
        [Range(0d, double.MaxValue, ErrorMessage = "Please enter a value")]
        [DisplayName("Entry Level:")]
        public decimal? instruction_entry { get; set; }
        
        // start date
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Start Date:")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string instruction_entry_date { get; set; }
        
        // exit level
        [DisplayName("Exit Level:")]
        public Nullable<decimal> instruction_exit { get; set; }

        // exit date
        [DisplayName("Exit Date:")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string instruction_exit_date { get; set; }

        // instruction
        [DisplayName("Instruction:")]
        public Nullable<int> instruction_type_id { get; set; }
        public List<Instruction_Type> InstructionTypes { get; set; }
        public string instruction_label { get; set; }

        // hedge instruction
        [DisplayName("Hedge Instructions:")]
        public Nullable<int> hedge_id { get; set; }
        public List<Hedge_Type> HedgeTypes { get; set; }

        // curency
        [DisplayName("Trade Execution Currency:")]
        public Nullable<int> currency_id { get; set; }
        public List<Currency> Currencies { get; set; }

        // supplementary info
        // linked trades
        [DisplayName("Linked Trades:")]
        public int[] related_trade_ids { get; set; }
        public List<Related_Trade> RelatedTrades { get; set; }



        public Trade_Performance TradePerformance { get; set; }
        // APL function
        [DisplayName("APL Function:")]
        public string apl_func { get; set; }

        // mark to mark rate
        [DisplayName("Mark to Market Rate:")]
        public string mark_to_mark_rate { get; set; }
        
        // internset rate diff
        [DisplayName("Interest Rate Differential:")]
        public string interest_rate_diff { get; set; }
        
        // abs performance
        public virtual ICollection<Measure_Type> MeasureTypes { get; set; }
        [DisplayName("Return:")]
        public Nullable<int> abs_measure_type_id { get; set; }
        public Nullable<int> abs_currency_id { get; set; }
        // rel performance
        [DisplayName("Return:")]
        public Nullable<int> rel_measure_type_id { get; set; }
        public Nullable<int> rel_currency_id { get; set; }
        [DisplayName("Relative to:")]
        public Nullable<int> return_benchmark_id { get; set; }
        // return value
        public string abs_return_value { get; set; }
        public string rel_return_value { get; set; }

        // comments
        [DisplayName("Comments:")]
        public string comments { get; set; }
        public Trade_Comment Comment { get; set; }

        //dropdowns for tradegroups
        public int trade_line_group_type_id { get; set; }
        public List<Trade_Line_Group_Type> TradeLineGroupTypes { get; set; } 
        
        //dropdowns for tradelines
        public int position_id { get; set; }
        public List<Position> Positions
        { get; set; }
        public int tradable_thing_id { get; set; }
        public List<Tradable_Thing> TradeTradableThings { get; set; }

        //for ko json response
        public List<TradeLineGroupViewModel> tradegroups { get; set; }
        public List<TradeLineViewModel> tradeLines { get; set; }
    }
}