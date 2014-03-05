using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradesWebApplication.ViewModels
{
    public class TradeInstructionDTO
    {
        public int trade_instruction_id
        {
            get;
            set;
        }
        public int trade_id
        {
            get;
            set;
        }
        public int instruction_type_id
        {
            get;
            set;
        }
        public string instruction_label
        {
            get;
            set;
        }
        public int instruction_entry
        {
            get;
            set;
        }
        public string instruction_entry_date
        {
            get;
            set;
        }
        public int? instruction_exit
        {
            get;
            set;
        }
        public string instruction_exit_date
        {
            get;
            set;
        }
        public int? hedge_id
        {
            get;
            set;
        }
        public int? currency_id
        {
            get;
            set;
        }
    }
}