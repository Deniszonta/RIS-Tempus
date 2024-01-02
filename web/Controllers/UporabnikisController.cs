using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UporabnikisController : Controller
    {
        private readonly TempusContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public UporabnikisController(TempusContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Uporabnikis
        public async Task<IActionResult> Index()
        {
              return View(await _context.Uporabniki.ToListAsync());
        }

        // GET: Uporabnikis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Uporabniki == null)
            {
                return NotFound();
            }

            var user = await _context.Uporabniki
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Uporabnikis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uporabnikis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Ime,Priimek,Email")] User user)
        {
            if (ModelState.IsValid){
                var appuser = new ApplicationUser{
                Ime = user.Ime,
                Priimek = user.Priimek,
                Email = user.Email,
                NormalizedEmail = "XXXX@GMAIL.COM",
                UserName = user.Email,
                NormalizedUserName = user.Email,
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!_context.Users.Any(u => u.Email == appuser.Email)){
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(appuser,"SpremeniMe123!");
                appuser.PasswordHash = hashed;
                _context.Users.Add(appuser);
            }

                _context.SaveChanges();
                var UserRoles = new IdentityUserRole<string>[]{
                    new IdentityUserRole<string>{RoleId = "3", UserId=appuser.Id},
                };

            foreach (IdentityUserRole<string> r in UserRoles){
                _context.UserRoles.Add(r);
            }
            _context.SaveChanges();

            user.appUser = appuser;
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
        return View(user);
    }

        // GET: Uporabnikis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Uporabniki == null)
            {
                return NotFound();
            }

            var user = await _context.Uporabniki.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Uporabnikis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Ime,Priimek,Email")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: Uporabnikis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Uporabniki == null)
            {
                return NotFound();
            }

            var user = await _context.Uporabniki
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Uporabnikis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Uporabniki == null)
            {
                return Problem("Entity set 'TempusContext.Uporabniki'  is null.");
            }
            var user = await _context.Uporabniki.FindAsync(id);
            if (user != null)
            {
                _context.Uporabniki.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return _context.Uporabniki.Any(e => e.UserID == id);
        }
    }
}
