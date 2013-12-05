using System.Collections.Generic;
using TradesDataModel;

namespace TradesViewModel
{
    public class TradeLineGroupViewModel
    {
        // group structure
        public int trade_line_group_type_id { get; set; }
        
        // canonical label

        public string trade_line_group_type_label { get; set; }

        // editorial label
        public string trade_line_group_editorial_label { get; set; }

        public List<TradeLineGroupType> TradeLineGroupTypes { get; set; }

        public List<TradeLines> TradeLines { get; set; }
       
    }
}