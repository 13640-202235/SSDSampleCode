using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProactiveControls.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(100), DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required, MinLength(2), MaxLength(100), DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, Phone, ProtectedPersonalData]
        public string Phone { get; set; }
        [Required, DataType(DataType.PostalCode), PersonalData, DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        [Required, CreditCard, NotMapped, ProtectedPersonalData, DisplayName("Credit Card")]
        public string CreditCard { get; set; }
        public string? HashedCreditCard { get; set; }
        [StringLength(256, ErrorMessage = "Notes can't exceed 256 chars")]
        public string Notes { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency), Range(0, 100_000_000_000)]
        public decimal? Assets { get; set; }
    }
}
