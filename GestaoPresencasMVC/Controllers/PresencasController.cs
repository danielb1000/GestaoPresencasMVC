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
    public class PresencasController : Controller
    {
        private readonly TentativaDb4Context _context;

        public PresencasController(TentativaDb4Context context)
        {
            _context = context;
        }

        // GET: Presencas
        public async Task<IActionResult> Index()
        {
            var tentativaDb4Context = _context.Presencas.Include(p => p.IdAlunoNavigation).Include(p => p.IdAulaNavigation);
            return View(await tentativaDb4Context.ToListAsync());
        }

        // GET: Presencas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presenca = await _context.Presencas
                .Include(p => p.IdAlunoNavigation)
                .Include(p => p.IdAulaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presenca == null)
            {
                return NotFound();
            }

            return View(presenca);
        }

        // GET: Presencas/Create
        public IActionResult Create()
        {
            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Id");
            ViewData["IdAula"] = new SelectList(_context.Aulas, "Id", "Id");
            return View();
        }

        // POST: Presencas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAula,IdAluno,Presente")] Presenca presenca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(presenca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Id", presenca.IdAluno);
            ViewData["IdAula"] = new SelectList(_context.Aulas, "Id", "Id", presenca.IdAula);
            return View(presenca);
        }

        // GET: Presencas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presenca = await _context.Presencas.FindAsync(id);
            if (presenca == null)
            {
                return NotFound();
            }
            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Id", presenca.IdAluno);
            ViewData["IdAula"] = new SelectList(_context.Aulas, "Id", "Id", presenca.IdAula);
            return View(presenca);
        }

        // POST: Presencas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAula,IdAluno,Presente")] Presenca presenca)
        {
            if (id != presenca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(presenca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresencaExists(presenca.Id))
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
            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Id", presenca.IdAluno);
            ViewData["IdAula"] = new SelectList(_context.Aulas, "Id", "Id", presenca.IdAula);
            return View(presenca);
        }

        // GET: Presencas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presenca = await _context.Presencas
                .Include(p => p.IdAlunoNavigation)
                .Include(p => p.IdAulaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presenca == null)
            {
                return NotFound();
            }

            return View(presenca);
        }

        // POST: Presencas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var presenca = await _context.Presencas.FindAsync(id);
            if (presenca != null)
            {
                _context.Presencas.Remove(presenca);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PresencaExists(int id)
        {
            return _context.Presencas.Any(e => e.Id == id);
        }
    }
}
