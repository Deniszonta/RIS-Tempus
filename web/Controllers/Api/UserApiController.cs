using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers_Api
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly TempusContext _context;

        public UserApiController(TempusContext context)
        {
            _context = context;
        }

        // GET: api/UserApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUporabniki()
        {
          if (_context.Uporabniki == null)
          {
              return NotFound();
          }
            return await _context.Uporabniki.ToListAsync();
        }

        // GET: api/UserApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Uporabniki == null)
          {
              return NotFound();
          }
            var user = await _context.Uporabniki.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/UserApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Uporabniki == null)
          {
              return Problem("Entity set 'TempusContext.Uporabniki'  is null.");
          }
            _context.Uporabniki.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }

        // DELETE: api/UserApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Uporabniki == null)
            {
                return NotFound();
            }
            var user = await _context.Uporabniki.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Uporabniki.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Uporabniki?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
    }
}
