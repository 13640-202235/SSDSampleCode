using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CryptographyEx.Data;
using CryptographyEx.Models;
using Microsoft.AspNetCore.DataProtection;

namespace CryptographyEx.Controllers
{
    public class BankAccountDpsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _protector;

        public BankAccountDpsController(ApplicationDbContext context, IDataProtectionProvider provider)
        {

            _context = context;
            _protector = provider.CreateProtector("BankAccounts");
        }



        // GET: BankAccountDps
        public async Task<IActionResult> Index()
        {
              return View(await _context.BankAccountDp.ToListAsync());
        }

        // GET: BankAccountDps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BankAccountDp == null)
            {
                return NotFound();
            }

            var bankAccountDp = await _context.BankAccountDp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccountDp == null)
            {
                return NotFound();
            }

            return View(bankAccountDp);
        }

        // GET: BankAccountDps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankAccountDps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number")] BankAccountDp bankAccountDp)
        {
            if (ModelState.IsValid)
            {
                bankAccountDp.EncryptedNumber = _protector.Protect(bankAccountDp.Number);
                bankAccountDp.DecryptedNumber = _protector.Unprotect(bankAccountDp.EncryptedNumber);

                _context.Add(bankAccountDp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccountDp);
        }

        // GET: BankAccountDps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BankAccountDp == null)
            {
                return NotFound();
            }

            var bankAccountDp = await _context.BankAccountDp.FindAsync(id);
            if (bankAccountDp == null)
            {
                return NotFound();
            }
            return View(bankAccountDp);
        }

        // POST: BankAccountDps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,EncryptedNumber,DecryptedNumber")] BankAccountDp bankAccountDp)
        {
            if (id != bankAccountDp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankAccountDp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAccountDpExists(bankAccountDp.Id))
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
            return View(bankAccountDp);
        }

        // GET: BankAccountDps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BankAccountDp == null)
            {
                return NotFound();
            }

            var bankAccountDp = await _context.BankAccountDp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccountDp == null)
            {
                return NotFound();
            }

            return View(bankAccountDp);
        }

        // POST: BankAccountDps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BankAccountDp == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BankAccountDp'  is null.");
            }
            var bankAccountDp = await _context.BankAccountDp.FindAsync(id);
            if (bankAccountDp != null)
            {
                _context.BankAccountDp.Remove(bankAccountDp);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankAccountDpExists(int id)
        {
          return _context.BankAccountDp.Any(e => e.Id == id);
        }
    }
}
