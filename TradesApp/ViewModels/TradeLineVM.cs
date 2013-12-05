using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TradesApp.ViewModels
{
    public class TradeLineVM
    {
        // position
        [Required(ErrorMessage = "required")]
        [DisplayName("Position")]
        public int position_id { get; set; }

        // financial insturment
        [Required(ErrorMessage = "required")]
        [DisplayName("Financial Instrument/Object")]
        public int tradable_thing_id { get; set; }

        public virtual ICollection<PositionDTO> Positions { get; set; }
        public virtual ICollection<TradableThingDTO> TradableThings { get; set; }
    }
}