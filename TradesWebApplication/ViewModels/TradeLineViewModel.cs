using System.Web.Razor.Generator;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.ViewModels
{
    public class TradeLineViewModel
    {
        public Trade_Line TradeLine { get; set; }
        public Position Position { get; set; }
        public Tradable_Thing TradeTradableThing { get; set; }
    }
}