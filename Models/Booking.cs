using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _16_Irsyad_ESDPROJ.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string FacilityDescription { get; set; }

        [Required]
        public DateTime BookingDateFrom { get; set; }

        [Required]
        public DateTime BookingDateTo { get; set; }

        [Required]
        public string BookedBy { get; set; }

        [Required]
        public string BookingStatus { get; set; }
    }
}
