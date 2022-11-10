using CryptographyEx.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptographyEx.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Password> Passwords { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<CryptographyEx.Models.BankAccountDp> BankAccountDp { get; set; }

    }
}