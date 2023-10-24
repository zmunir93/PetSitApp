namespace PetSitApp.DTOs.GoogleGeocoding
{
    public class GeocodeResponse
    {
        public GeocodeResult[] Results { get; set; }
        public string Status { get; set; }
    }
}
