using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoPresencasMVC.Models;
using GestaoPresencasMVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace GestaoPresencasMVC.Controllers
{
    public class AnoesController : BaseController
    {
        private readonly TentativaDb4Context _context;

        public AnoesController(TentativaDb4Context context, UserManager<gpUser> userManager)
             : base(userManager)
        {
            _context = context;
        }

        // GET: Anoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Anos.ToListAsync());
        }

        // GET: Anoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ano = await _context.Anos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ano == null)
            {
                return NotFound();
            }

            return View(ano);
        }

        // GET: Anoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Anoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero")] Ano ano)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ano);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ano);
        }

        // GET: Anoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ano = await _context.Anos.FindAsync(id);
            if (ano == null)
            {
                return NotFound();
            }
            return View(ano);
        }

        // POST: Anoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero")] Ano ano)
        {
            if (id != ano.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ano);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnoExists(ano.Id))
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
            return View(ano);
        }

        // GET: Anoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ano = await _context.Anos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ano == null)
            {
                return NotFound();
            }

            return View(ano);
        }

        // POST: Anoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ano = await _context.Anos.FindAsync(id);
            if (ano != null)
            {
                _context.Anos.Remove(ano);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnoExists(int id)
        {
            return _context.Anos.Any(e => e.Id == id);
        }
    }
}
