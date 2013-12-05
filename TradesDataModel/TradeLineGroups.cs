namespace TradesDataModel
{
    public class TradeLineGroups
    {
        //public TradeLineGroups()
        //{
        //    this.Trade_Line = new HashSet<TradeLines>();
        //}
    
        public int TradeLineGroupId { get; set; }
        public int? TradeLineGroupTypeId { get; set; }
        public string TradeLineGroupUri { get; set; }
        public string TradeLineGroupLabel { get; set; }
        public string TradeLineGroupEditorialLabel { get; set; }
    
        //public virtual ICollection<TradeLines> Trade_Line { get; set; }
        //public virtual Trade_Line_Group_Type Trade_Line_Group_Type { get; set; }
    }
}
