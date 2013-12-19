using System.Collections.Generic;
using System.Web.Razor.Generator;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.ViewModels
{
    public class TradeLineViewModel
    {
        public Trade_Line TradeLine { get; set; }
        public Position Position { get; set; }
        public virtual ICollection<Position> Positions
        { get; set; }
        public Tradable_Thing TradeTradableThing { get; set; }
        public virtual ICollection<Tradable_Thing> TradeTradableThings { get; set; }

        //for json
        public int trade_line_id { get; set; }
        public int position_id { get; set; }
        public int tradable_thing_id { get; set; }
        
    }
}