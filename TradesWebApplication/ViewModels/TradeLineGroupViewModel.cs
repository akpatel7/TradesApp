using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.ViewModels
{
    public class TradeLineGroupViewModel
    {
        public Trade_Line_Group TradeLineGroup { get; set; }
        [DisplayName("Group Structure")]
        public Trade_Line_Group_Type TradeLineGroupType { get; set; }
        public virtual ICollection<Trade_Line_Group_Type> TradeLineGroupTypes { get; set; } 
        [DisplayName("Editorial Label")]
        public string EditorialLabel { get; set; }
        [DisplayName("Canonical Label")]
        public string CanonicalLabel { get; set; }
    }
}