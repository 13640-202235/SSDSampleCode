using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CryptographyEx.Data;
using CryptographyEx.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Scripting;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace CryptographyEx.Controllers
{
    public class PasswordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _usermanager;

        public PasswordsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Passwords
        public async Task<IActionResult> Index()
        {
              return View(await _context.Passwords.ToListAsync());
        }

        // GET: Passwords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Passwords == null)
            {
                return NotFound();
            }

            var password = await _context.Passwords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (password == null)
            {
                return NotFound();
            }

            return View(password);
        }

        // GET: Passwords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passwords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlainTextPassword")] Password password)
        {
            if (ModelState.IsValid)
            {
                password = HashSaltPassword(password);
                _context.Add(password);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(password);
        }

        private Password HashSaltPassword(Password password)
        {
            //update hashed password
            SHA512Managed sha = new SHA512Managed();
            byte[] input = Encoding.UTF8.GetBytes(password.PlainTextPassword);
            byte[] output = sha.ComputeHash(input);
            password.HashedPassword = Convert.ToBase64String(output);

            //generate random salt
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomSalt = new byte[32];
            rng.GetBytes(randomSalt);
            password.Salt = Convert.ToBase64String(randomSalt);

            //update hashed random salt and password
            input = Encoding.UTF8.GetBytes(password.PlainTextPassword + password.Salt);
            output = sha.ComputeHash(input);
            password.HashedSaltedPassword = Convert.ToBase64String(output);

            //bcrypt pasword
            password.BcryptPassword = BCrypt.Net.BCrypt.HashPassword(password.PlainTextPassword);

            //update userId
            password.UserId = _usermanager.GetUserId(User);

            return password;
        }

        // GET: Passwords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Passwords == null)
            {
                return NotFound();
            }

            var password = await _context.Passwords.FindAsync(id);
            if (password == null)
            {
                return NotFound();
            }
            return View(password);
        }

        // POST: Passwords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,PlainTextPassword,HashedPassword,Salt,HashedSaltedPassword,BcryptPassword")] Password password)
        {
            if (id != password.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(password);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasswordExists(password.Id))
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
            return View(password);
        }

        // GET: Passwords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Passwords == null)
            {
                return NotFound();
            }

            var password = await _context.Passwords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (password == null)
            {
                return NotFound();
            }

            return View(password);
        }

        // POST: Passwords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Passwords == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Passwords'  is null.");
            }
            var password = await _context.Passwords.FindAsync(id);
            if (password != null)
            {
                _context.Passwords.Remove(password);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasswordExists(int id)
        {
          return _context.Passwords.Any(e => e.Id == id);
        }
    }
}
