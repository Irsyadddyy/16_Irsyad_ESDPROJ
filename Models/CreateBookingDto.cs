using System.ComponentModel.DataAnnotations;

public class CreateBookingDto
{
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