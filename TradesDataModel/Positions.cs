namespace TradesDataModel
{
    public class Positions
    {
        //public Positions()
        //{
        //    this.Trade_Line = new HashSet<Trade_Line>();
        //}
    
        public int PositionId { get; set; }
        public string PositionUri { get; set; }
        public string PositionLabel { get; set; }
        public int? PositionRelativityId { get; set; }
    
        //public virtual Relativity Relativity { get; set; }
        //public virtual ICollection<Trade_Line> Trade_Line { get; set; }
    }
}
