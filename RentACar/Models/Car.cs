using System.ComponentModel.DataAnnotations;

namespace RentACar.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Range(1950, 2100)]
        public int Year { get; set; }

        [Range(1, 20)]
        public int Seats { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(typeof(decimal), "0.01", "1000000")]
        public decimal PricePerDay { get; set; }
    }
}