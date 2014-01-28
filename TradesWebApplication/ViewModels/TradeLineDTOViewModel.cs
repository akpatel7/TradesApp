using System.Collections.Generic;
using System.Web.Razor.Generator;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.ViewModels
{
    public class TradeLineDTOViewModel
    {
        //for json
        public int trade_line_id { get; set; }
        public int position_id { get; set; }
        public int tradable_thing_id { get; set; }
        
    }
}