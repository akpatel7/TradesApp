//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradesWebApplication.DAL.EFModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Position
    {
        public Position()
        {
            this.Trade_Line = new HashSet<Trade_Line>();
        }
    
        public int position_id { get; set; }
        public string position_uri { get; set; }
        public string position_label { get; set; }
        public Nullable<int> position_relativity_id { get; set; }
    
        public virtual Relativity Relativity { get; set; }
        public virtual ICollection<Trade_Line> Trade_Line { get; set; }
    }
}
