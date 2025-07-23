using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeygirMuhendisi.Web.Data;
using BeygirMuhendisi.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace BeygirMuhendisi.Web.Controllers
{
    public class TahminlerController : Controller
    {
        private readonly UygulamaDbContext _context;

        public TahminlerController(UygulamaDbContext context)
        {
            _context = context;
        }

        // GET: Tahminler
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tahminler.ToListAsync());
        }

        // GET: Tahminler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tahmin = await _context.Tahminler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tahmin == null)
            {
                return NotFound();
            }

            return View(tahmin);
        }

        // GET: Tahminler/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tahminler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tarih,Baslik,Pist,Gun,Tur,Aciklama,SheetsUrl,Aktif,TutanTahmin")] Tahmin tahmin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tahmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tahmin);
        }

        // GET: Tahminler/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tahmin = await _context.Tahminler.FindAsync(id);
            if (tahmin == null)
            {
                return NotFound();
            }
            return View(tahmin);
        }

        // POST: Tahminler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tarih,Baslik,Pist,Gun,Tur,Aciklama,SheetsUrl,Aktif,TutanTahmin")] Tahmin tahmin)
        {
            if (id != tahmin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tahmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TahminExists(tahmin.Id))
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
            return View(tahmin);
        }

        // GET: Tahminler/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tahmin = await _context.Tahminler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tahmin == null)
            {
                return NotFound();
            }

            return View(tahmin);
        }

        // POST: Tahminler/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tahmin = await _context.Tahminler.FindAsync(id);
            if (tahmin != null)
            {
                _context.Tahminler.Remove(tahmin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TahminExists(int id)
        {
            return _context.Tahminler.Any(e => e.Id == id);
        }

        // GET: Tahminler/TutanTahminler
        public async Task<IActionResult> TutanTahminler()
        {
            // Sadece tutan tahminleri listele
            var tutanlar = await _context.Tahminler
                .Where(t => t.TutanTahmin)
                .ToListAsync();
            return View(tutanlar);
        }
    }
}
