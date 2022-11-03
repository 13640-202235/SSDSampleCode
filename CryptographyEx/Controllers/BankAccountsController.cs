using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CryptographyEx.Data;
using CryptographyEx.Models;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyEx.Controllers
{
    public class BankAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public BankAccountsController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

        }

        // GET: BankAccounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.BankAccounts.ToListAsync());
        }

        // GET: BankAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number")] BankAccount bankAccount)
        {
            bankAccount = EncryptAndDecrypt(bankAccount);
            if (ModelState.IsValid)
            {
                _context.Add(bankAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccount);
        }

        private BankAccount EncryptAndDecrypt(BankAccount bankAccount)
        {
            Aes aes = AesInitialize(); //Aes.Create();

            //encrypt bank account number
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] input = Encoding.UTF8.GetBytes(bankAccount.Number);
            byte[] output = encryptor.TransformFinalBlock(input, 0, input.Length);
            bankAccount.EncryptedNumber = Convert.ToBase64String(output);

            //decrypt bank account number
            ICryptoTransform decryptor = aes.CreateDecryptor();
            input = Convert.FromBase64String(bankAccount.EncryptedNumber);
            output = decryptor.TransformFinalBlock(input, 0, input.Length);
            bankAccount.DecryptedNumber = Encoding.UTF8.GetString(output);

            return bankAccount;
        }

        private Aes AesInitialize()
        {
            Aes aes = Aes.Create();

            // retrieve key & IV from secrets in our configuration
            var aesSettings = _config.GetSection("AES").Get<AESSettings>();

            if (aesSettings == null)
            {
                string key = null, IV = null; // prepare Configuration strings for Key and IV
                foreach (byte b in aes.Key)
                    key += $"{ b},";
                key = key.Trim(',');

                foreach (byte b in aes.IV)
                    IV += $"{ b},";
                IV = IV.Trim(',');
            }
            else
            {
                aes.Key = aesSettings.Key;
                aes.IV = aesSettings.IV;
            }

            return aes;
        }


        // GET: BankAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,EncryptedNumber,DecryptedNumber")] BankAccount bankAccount)
        {
            if (id != bankAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAccountExists(bankAccount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BankAccounts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BankAccounts'  is null.");
            }
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount != null)
            {
                _context.BankAccounts.Remove(bankAccount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankAccountExists(int id)
        {
            return _context.BankAccounts.Any(e => e.Id == id);
        }
    }
}
