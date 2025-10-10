using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Admission_System.Areas.Identity.Data;
using Student_Admission_System.Models;

namespace Student_Admission_System.Controllers
{
    public class AdmissionStatusController : Controller
    {
        private readonly ApplicationDbContext2 _context;

        public AdmissionStatusController(ApplicationDbContext2 context)
        {
            _context = context;
        }

        // GET: AdmissionStatus
        public async Task<IActionResult> Index()
        {
              return _context.AdmissionStatuses != null ? 
                          View(await _context.AdmissionStatuses.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.AdmissionStatuses'  is null.");
        }

        // GET: AdmissionStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdmissionStatuses == null)
            {
                return NotFound();
            }

            var admissionStatus = await _context.AdmissionStatuses
                .FirstOrDefaultAsync(m => m.AdmissionStatusID == id);
            if (admissionStatus == null)
            {
                return NotFound();
            }

            return View(admissionStatus);
        }

        // GET: AdmissionStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdmissionStatus/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdmissionStatusID,Name")] AdmissionStatus admissionStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admissionStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admissionStatus);
        }

        // GET: AdmissionStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdmissionStatuses == null)
            {
                return NotFound();
            }

            var admissionStatus = await _context.AdmissionStatuses.FindAsync(id);
            if (admissionStatus == null)
            {
                return NotFound();
            }
            return View(admissionStatus);
        }

        // POST: AdmissionStatus/Edit/
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdmissionStatusID,Name")] AdmissionStatus admissionStatus)
        {
            if (id != admissionStatus.AdmissionStatusID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admissionStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdmissionStatusExists(admissionStatus.AdmissionStatusID))
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
            return View(admissionStatus);
        }

        // GET: AdmissionStatus/Delete/
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdmissionStatuses == null)
            {
                return NotFound();
            }

            var admissionStatus = await _context.AdmissionStatuses
                .FirstOrDefaultAsync(m => m.AdmissionStatusID == id);
            if (admissionStatus == null)
            {
                return NotFound();
            }

            return View(admissionStatus);
        }

        // POST: AdmissionStatus/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdmissionStatuses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AdmissionStatuses'  is null.");
            }
            var admissionStatus = await _context.AdmissionStatuses.FindAsync(id);
            if (admissionStatus != null)
            {
                _context.AdmissionStatuses.Remove(admissionStatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdmissionStatusExists(int id)
        {
          return (_context.AdmissionStatuses?.Any(e => e.AdmissionStatusID == id)).GetValueOrDefault();
        }
    }
}
