using System.ComponentModel.DataAnnotations;

namespace RentACar.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}