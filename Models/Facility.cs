using System.ComponentModel.DataAnnotations;

namespace _16_Irsyad_ESDPROJ.Models
{
    public class Facility
    {
        [Key]
        public int FacilityID { get; set; }
        public string? FacilityName { get; set; }
        public string? Description { get; set; }
        public int Seatings { get; set; }
    }
}