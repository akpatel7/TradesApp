using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.ViewModels
{
    public class TradeLineGroupViewModel
    {
        public Trade_Line_Group TradeLineGroup { get; set; }
        public Trade_Line_Group_Type TradeLineGroupType { get; set; }
        public string EditorialLabel { get; set; }
        public string CanonicalLabel { get; set; }
    }
}