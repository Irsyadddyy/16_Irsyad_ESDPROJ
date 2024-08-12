namespace _16_Irsyad_ESDPROJ.Models
{
    public class BookingUpdateDto
    {
        public string FacilityDescription { get; set; }
        public DateTime BookingDateFrom { get; set; }
        public DateTime BookingDateTo { get; set; }
        public string BookedBy { get; set; }
        public string BookingStatus { get; set; }
    }
}
