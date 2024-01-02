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



namespace web.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly TempusContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public TicketsController(TempusContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userInRole = await _userManager.IsInRoleAsync(currentUser, "Administrator");
            var tempusContext1 = _context.Tickets.Include(t => t.Project).Include(t => t.User);
            if (userInRole){
                return View(await tempusContext1.ToListAsync());
            }
            var userId = _context.Uporabniki.Single(t => t.appUser == currentUser).UserID;
            var tempusContext = _context.Tickets.Include(t => t.Project).Include(t => t.User).Where(t => t.UserID == userId);
            return View(await tempusContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Project)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Ime");
            ViewData["UserID"] = new SelectList(_context.Uporabniki, "UserID", "Email");
            ViewData["Stanje"] = new SelectList(Enum.GetValues(typeof(StanjeTicketa)));
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketID,Naslov,Opis,Stanje,UserID,ProjectID,cas")] Ticket ticket)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {   
                ticket.appUser = currentUser;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Ime", ticket.ProjectID);
            ViewData["UserID"] = new SelectList(_context.Uporabniki, "UserID", "Email", ticket.UserID);
            ViewData["Stanje"] = new SelectList(Enum.GetValues(typeof(StanjeTicketa)));
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Ime", ticket.ProjectID);
            ViewData["UserID"] = new SelectList(_context.Uporabniki, "UserID", "Email", ticket.UserID);
            ViewData["Stanje"] = new SelectList(Enum.GetValues(typeof(StanjeTicketa)));
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketID,Naslov,Opis,Stanje,UserID,ProjectID,cas")] Ticket ticket)
        {
            if (id != ticket.TicketID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketID))
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
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Ime", ticket.ProjectID);
            ViewData["UserID"] = new SelectList(_context.Uporabniki, "UserID", "Email", ticket.UserID);
            ViewData["Stanje"] = new SelectList(Enum.GetValues(typeof(StanjeTicketa)));

            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Project)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'TempusContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return _context.Tickets.Any(e => e.TicketID == id);
        }
    }
}
