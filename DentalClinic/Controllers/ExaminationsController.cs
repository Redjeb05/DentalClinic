using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentalClinic.Data;

namespace DentalClinic.Controllers
{
    public class ExaminationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExaminationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Examination.Include(e => e.Dentist).Include(e => e.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Examination == null)
            {
                return NotFound();
            }

            var examination = await _context.Examination
                .Include(e => e.Dentist)
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examination == null)
            {
                return NotFound();
            }

            return View(examination);
        }

        public IActionResult Create()
        {
            ViewData["DentistId"] = new SelectList(_context.Dentist, "DentistId", "FullName");
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,DateAndHour,DentistId,Description")] Examination examination)
        {
            if (ModelState.IsValid)
            {
                if (await ExaminationExistsForDentistAndPatient(examination.DentistId, examination.PatientId, examination.DateAndHour))
                {
                    ModelState.AddModelError("DateAndHour", "Another examination already exists for the same dentist and patient at this date and time.");
                    ViewData["DentistId"] = new SelectList(_context.Dentist, "DentistId", "FullName", examination.DentistId);
                    ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "FullName", examination.PatientId);
                    return View(examination);
                }

                _context.Add(examination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DentistId"] = new SelectList(_context.Dentist, "DentistId", "FullName", examination.DentistId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "FullName", examination.PatientId);
            return View(examination);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Examination == null)
            {
                return NotFound();
            }

            var examination = await _context.Examination.FindAsync(id);
            if (examination == null)
            {
                return NotFound();
            }
            ViewData["DentistId"] = new SelectList(_context.Dentist, "DentistId", "FullName", examination.DentistId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "FullName", examination.PatientId);
            return View(examination);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,DateAndHour,DentistId,Description")] Examination examination)
        {
            if (id != examination.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (await ExaminationExistsForDentistAndPatient(examination.DentistId, examination.PatientId, examination.DateAndHour, id))
                {
                    ModelState.AddModelError("DateAndHour", "Another examination already exists for the same dentist and patient at this date and time.");
                    ViewData["DentistId"] = new SelectList(_context.Dentist, "DentistId", "FullName", examination.DentistId);
                    ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "FullName", examination.PatientId);
                    return View(examination);
                }

                try
                {
                    _context.Update(examination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExaminationExists(examination.Id))
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
            ViewData["DentistId"] = new SelectList(_context.Dentist, "DentistId", "FullName", examination.DentistId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "FullName", examination.PatientId);
            return View(examination);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Examination == null)
            {
                return NotFound();
            }

            var examination = await _context.Examination
                .Include(e => e.Dentist)
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examination == null)
            {
                return NotFound();
            }

            return View(examination);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Examination == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Examination'  is null.");
            }
            var examination = await _context.Examination.FindAsync(id);
            if (examination != null)
            {
                _context.Examination.Remove(examination);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExaminationExists(int id)
        {
            return (_context.Examination?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<bool> ExaminationExistsForDentistAndPatient(int dentistId, int patientId, DateTime dateAndHour, int? excludeId = null)
        {
            return await _context.Examination
                .AnyAsync(e => e.DentistId == dentistId && e.PatientId == patientId && e.DateAndHour == dateAndHour && (excludeId == null || e.Id != excludeId));
        }
    }
}
