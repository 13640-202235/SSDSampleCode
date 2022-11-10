using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CryptographyEx.Models
{
    public class BankAccountDp
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        [ReadOnly(true)]
        public string? EncryptedNumber { get; set; }
        [ReadOnly(true)]
        public string? DecryptedNumber { get; set; }

    }
}
