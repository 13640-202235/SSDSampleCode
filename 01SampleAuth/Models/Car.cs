using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _01SampleAuth.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required, Range(2000, 2030)]
        public int Year { get; set; }
        public string Color { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency), Range(100, 100000)]
        public decimal? Price { get; set; }
    }
}
