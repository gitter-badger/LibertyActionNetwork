namespace Li.Lan.Models
{
    public class Precinct
    {
        public int PrecinctId { get; set; }
        public byte ActiveStatus { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string County { get; set; }
    }
}