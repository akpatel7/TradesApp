using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.ViewModels
{
    public class TradesDTOViewModel
    {
        // Trade
        public int trade_id { get; set; }

        // service
        public int service_id { get; set; }
        
        // trade type
        public int length_type_id { get; set; }
      
        // benchmark
        public int relativity_id { get; set; }
        
        // benchmark selection
        public int? benchmark_id { get; set; }
        
        // update time
        public string last_updated { get; set; }

        // last updated by
        public int last_updated_by { get; set; }
        
        // canonical label
        public string trade_label { get; set; }
        
        // editorial label
        public string trade_editorial_label { get; set; }
        
        // trade structure
        public int structure_type_id { get; set; }

        //trade uri
        public string trade_uri { get; set; }

        //status
        public int? status { get; set; }
        
        //------------------------------------
        //dbo.Trade_Instruction
        public int trade_instruction_id { get; set; }
        
        // entry level
        public decimal? instruction_entry { get; set; }
        
        // start date
        public string instruction_entry_date { get; set; }
        
        // exit level
        public Nullable<decimal> instruction_exit { get; set; }

        // exit date
        public string instruction_exit_date { get; set; }

        // instruction
        public Nullable<int> instruction_type_id { get; set; }
        
        public string instruction_label { get; set; }

        // hedge instruction
        public Nullable<int> hedge_id { get; set; }

        // curency
        [DisplayName("Trade Execution Currency:")]
        public Nullable<int> currency_id { get; set; }


        // supplementary info
        // linked trades
        public int[] related_trade_ids { get; set; }
        public string related_trade_ids_list { get; set; }

        // APL function
        public string apl_func { get; set; }

        // mark to mark rate
        public int mark_track_record_id { get; set; }
        public string mark_to_mark_rate { get; set; }
        
        // intrest rate diff
        public int int_track_record_id { get; set; }
        public string interest_rate_diff { get; set; }
        
        // abs performance
        public int abs_track_performance_id { get; set; }
        public Nullable<int> abs_measure_type_id { get; set; }
        public Nullable<int> abs_currency_id { get; set; }
        // rel performance
        public int rel_track_performance_id { get; set; }
        public Nullable<int> rel_measure_type_id { get; set; }
        public Nullable<int> rel_currency_id { get; set; }
        public Nullable<int> return_benchmark_id { get; set; }
        // return value
        public string abs_return_value { get; set; }
        public string rel_return_value { get; set; }

        // comments
        public int comment_id { get; set; }
        public string comments { get; set; }

        //for ko json response
        public List<TradeLineGroupDTOViewModel> tradegroups { get; set; }
        public string CRUDMode { get; set; }
        //public List<TradeLineDTOViewModel> tradeLines { get; set; }
    }
}