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
        public IEnumerable<Trade> Trades { get; set; }
        public IEnumerable<Trade_Line_Group> TradeLineGroups { get; set; }
        public IEnumerable<Trade_Line> TradeLines { get; set; }

        //public Benchmark Benchmark { get; set; }
        //public Currency Currency { get; set; }
        //public Length_Type LengthType { get; set; }     
        //public Relativity Relativity { get; set; }
        //public Service Service { get; set; }
        //public Status Status { get; set; }
        //public Structure_Type StructureType { get; set; }
       
        //public Trade_Instruction TradeInstruction { get; set; }
        //public Hedge_Type HedgeInstruction{ get; set; }
        //public IEnumerable<Track_Record> TrackRecord { get; set; }
        //public IEnumerable<Trade_Comment> TradeComment { get; set; }
        
        //public IEnumerable<Trade_Performance> TradePerformance { get; set; }

        //public IEnumerable<Related_Trade> RelatedTrades { get; set; }

        // Trade
        
        // service
        [Required(ErrorMessage = "required")]
        [DisplayName("Service")]
        public int service_id { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        
        // trade type
        [Required(ErrorMessage = "required")]
        [DisplayName("Trade Type")]
        public int length_type_id { get; set; }
        public virtual ICollection<Length_Type> Length_Types { get; set; }
        
        // benchmark
        [Required(ErrorMessage = "required")]
        [DisplayName("Benchmark")]
        public int relativity_id { get; set; }
        public virtual ICollection<Relativity> Relativitys { get; set; }
        
        // benchmark selection
        [DisplayName("Benchmark Selection")]
        public int benchmark_id { get; set; }
        public virtual ICollection<Benchmark> Benchmarks { get; set; }
        
        // update time
        [Required(ErrorMessage = "required")]
        [DisplayName("System Updated/Created Date")]
        public string created_on { get; set; }
        
        // canonical label
        [DisplayName("Canonical Label")]
        public string trade_label { get; set; }
        
        // editorial label
        [DisplayName("Editorial Label")]
        public string trade_editorial_label { get; set; }
        
        // trade structure
        [Required(ErrorMessage = "required")]
        [DisplayName("Trade Structure")]
        public int structure_type_id { get; set; }
        public virtual ICollection<Structure_Type> Structure_Types { get; set; }

        
        //------------------------------------
        // entry level
        [Required(ErrorMessage = "required")]
        [DisplayName("Entry Level")]
        public decimal instruction_entry { get; set; }
        
        // start date
        [Required(ErrorMessage = "required")]
        [DisplayName("Start Date")]
        public string instruction_entry_date { get; set; }
        
        // exit level
        [DisplayName("Exit Level")]
        public Nullable<decimal> instruction_exit { get; set; }

        // exit date
        [DisplayName("Exit Date")]
        public string instruction_exit_date { get; set; }

        // instruction
        [DisplayName("Instruction")]
        public Nullable<int> instruction_type_id { get; set; }
        public virtual ICollection<Instruction_Type> Instruction_Types { get; set; }
        public string instruction_label { get; set; }

        // hedge instruction
        [DisplayName("Hedge Instructions")]
        public Nullable<int> hedge_id { get; set; }
        public virtual ICollection<Hedge_Type> Hedge_Types { get; set; }

        // curency
        [DisplayName("Trade Execution Currency")]
        public Nullable<int> currency_id { get; set; }
        public virtual ICollection<Currency> Currencies { get; set; }

        // supplementary info
        // linked trades
        [DisplayName("Linked Trades")]
        public Nullable<int> related_trade_id { get; set; }
        public virtual ICollection<Trade> Releated_Trades { get; set; }

        // APL function
        [DisplayName("APL Function")]
        public string apl_func { get; set; }

        // mark to mark rate
        [DisplayName("Mark to Market Rate")]
        public string mark_to_mark_rate { get; set; }
        
        // internset rate diff
        [DisplayName("Interest Rate Differential")]
        public string interest_rate_diff { get; set; }
        
        // abs performance
        public virtual ICollection<Measure_Type> Measure_Types { get; set; }
        [DisplayName("Return")]
        public Nullable<int> abs_measure_type_id { get; set; }
        public Nullable<int> abs_currency_id { get; set; }
        // rel performance
        [DisplayName("Return")]
        public Nullable<int> rel_measure_type_id { get; set; }
        public Nullable<int> rel_currency_id { get; set; }
        [DisplayName("Relative to")]
        public Nullable<int> return_benchmark_id { get; set; }
        // return value
        public string abs_return_value { get; set; }
        public string rel_return_value { get; set; }

        // comments
        [DisplayName("Comments")]
        public string comments { get; set; }

    }
}