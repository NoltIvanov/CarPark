namespace beyond.park.client.Models.Rest.Carpark {
    public sealed class CarparkBody {
        public string Name { get; set; }

        public string Address { get; set; }
        
        public int Distance { get; set; }

        public decimal Fee { get; set; }
    }
}
