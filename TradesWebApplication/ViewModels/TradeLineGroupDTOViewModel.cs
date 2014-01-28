using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.ViewModels
{
    public class TradeLineGroupDTOViewModel
    {
        //for json
        public int trade_line_group_id { get; set; }
        [DisplayName("Group Structure")]
        public int trade_line_group_type_id { get; set; }
        [DisplayName("Editorial Label")]
        public string trade_line_group_editorial_label { get; set; }
        [DisplayName("Canonical Label")]
        public string trade_line_group_label { get; set; }
        public List<TradeLineDTOViewModel> tradeLines { get; set; }
    }
}