using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentalClinic.Data;
using Microsoft.AspNetCore.Authorization;

namespace DentalClinic.Controllers
{
    [Authorize]
    public class DentistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DentistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dentists
        public async Task<IActionResult> Index()
        {
              return _context.Dentist != null ? 
                          View(await _context.Dentist.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Dentist'  is null.");
        }

        // GET: Dentists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dentist == null)
            {
                return NotFound();
            }

            var dentist = await _context.Dentist
                .FirstOrDefaultAsync(m => m.DentistId == id);
            if (dentist == null)
            {
                return NotFound();
            }

            return View(dentist);
        }

        // GET: Dentists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dentists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DentistId,Name,LastName,Description")] Dentist dentist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dentist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dentist);
        }

        // GET: Dentists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dentist == null)
            {
                return NotFound();
            }

            var dentist = await _context.Dentist.FindAsync(id);
            if (dentist == null)
            {
                return NotFound();
            }
            return View(dentist);
        }

        // POST: Dentists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DentistId,Name,LastName,Description")] Dentist dentist)
        {
            if (id != dentist.DentistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dentist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DentistExists(dentist.DentistId))
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
            return View(dentist);
        }

        // GET: Dentists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dentist == null)
            {
                return NotFound();
            }

            var dentist = await _context.Dentist
                .FirstOrDefaultAsync(m => m.DentistId == id);
            if (dentist == null)
            {
                return NotFound();
            }

            return View(dentist);
        }

        // POST: Dentists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dentist == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Dentist'  is null.");
            }
            var dentist = await _context.Dentist.FindAsync(id);
            if (dentist != null)
            {
                _context.Dentist.Remove(dentist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DentistExists(int id)
        {
          return (_context.Dentist?.Any(e => e.DentistId == id)).GetValueOrDefault();
        }
    }
}
