using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoPresencasMVC.Models;

namespace GestaoPresencasMVC.Controllers
{
    public class UcsController : Controller
    {
        private readonly TentativaDb4Context _context;

        public UcsController(TentativaDb4Context context)
        {
            _context = context;
        }

        // GET: Ucs
        public async Task<IActionResult> Index()
        {
            var tentativaDb4Context = _context.Ucs.Include(u => u.IdCursoNavigation).Include(u => u.IdDocenteNavigation);
            return View(await tentativaDb4Context.ToListAsync());
        }

        // GET: Ucs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uc = await _context.Ucs
                .Include(u => u.IdCursoNavigation)
                .Include(u => u.IdDocenteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uc == null)
            {
                return NotFound();
            }

            return View(uc);
        }

        // GET: Ucs/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Nome");
            ViewData["IdDocente"] = new SelectList(_context.Docentes, "Id", "Nome");
            return View();
        }

        // POST: Ucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdDocente,IdCurso,Nome")] Uc uc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", uc.IdCurso);
            ViewData["IdDocente"] = new SelectList(_context.Docentes, "Id", "Id", uc.IdDocente);
            return View(uc);
        }

        // GET: Ucs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uc = await _context.Ucs.FindAsync(id);
            if (uc == null)
            {
                return NotFound();
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Nome", uc.IdCurso);
            ViewData["IdDocente"] = new SelectList(_context.Docentes, "Id", "Nome", uc.IdDocente);
            return View(uc);
        }

        // POST: Ucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdDocente,IdCurso,Nome")] Uc uc)
        {
            if (id != uc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UcExists(uc.Id))
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
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", uc.IdCurso);
            ViewData["IdDocente"] = new SelectList(_context.Docentes, "Id", "Id", uc.IdDocente);
            return View(uc);
        }

        // GET: Ucs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uc = await _context.Ucs
                .Include(u => u.IdCursoNavigation)
                .Include(u => u.IdDocenteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uc == null)
            {
                return NotFound();
            }

            return View(uc);
        }

        // POST: Ucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uc = await _context.Ucs.FindAsync(id);
            if (uc != null)
            {
                _context.Ucs.Remove(uc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UcExists(int id)
        {
            return _context.Ucs.Any(e => e.Id == id);
        }
    }
}
