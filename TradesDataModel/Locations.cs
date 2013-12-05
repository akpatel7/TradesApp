namespace TradesDataModel
{
    public class Locations
    {
        //public Locations()
        //{
        //    this.Tradable_Thing = new HashSet<Tradable_Thing>();
        //}
    
        public int LocationId { get; set; }
        public string LocationUri { get; set; }
        public string LocationCode { get; set; }
        public string LocationLabel { get; set; }
    
        //public virtual ICollection<Tradable_Thing> Tradable_Thing { get; set; }
    }
}
