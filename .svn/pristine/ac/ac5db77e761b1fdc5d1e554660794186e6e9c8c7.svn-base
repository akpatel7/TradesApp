using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TradesApp.Models;

namespace TradesApp.ViewModels
{
    public class TradeLineGroupVM
    {
        // group structure
        [Required(ErrorMessage = "required")]
        [DisplayName("Group Structure")]
        public int trade_line_group_type_id { get; set; }
        
        // canonical label
        [DisplayName("Canonical Label")]
        public string trade_line_group_type_label { get; set; }

        // editorial label
        [DisplayName("Editorial Label")]
        public string trade_line_group_editorial_label { get; set; }

        public virtual ICollection<TradeLineGroupTypeDTO> TradeLineGroupTypes { get; set; }

        public virtual ICollection<TradeLineVM> TradeLines { get; set; }
       
    }
}