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
    public class AlunoUcsController : Controller
    {
        private readonly TentativaDb4Context _context;

        public AlunoUcsController(TentativaDb4Context context)
        {
            _context = context;
        }

        // GET: AlunoUcs
        public async Task<IActionResult> Index()
        {
            var tentativaDb4Context = _context.AlunoUcs.Include(a => a.IdAlunoNavigation).Include(a => a.IdUcNavigation);
            return View(await tentativaDb4Context.ToListAsync());
        }

        // GET: AlunoUcs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alunoUc = await _context.AlunoUcs
                .Include(a => a.IdAlunoNavigation)
                .Include(a => a.IdUcNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alunoUc == null)
            {
                return NotFound();
            }

            return View(alunoUc);
        }

        // GET: AlunoUcs/Create
        public IActionResult Create()
        {
            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Nome");
            ViewData["IdUc"] = new SelectList(_context.Ucs, "Id", "Nome");
            return View();
        }

        // POST: AlunoUcs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUc,IdCurso,IdAluno")] AlunoUc alunoUc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alunoUc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Id", alunoUc.IdAluno);
            ViewData["IdUc"] = new SelectList(_context.Ucs, "Id", "Id", alunoUc.IdUc);
            return View(alunoUc);
        }

        // GET: AlunoUcs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alunoUc = await _context.AlunoUcs.FindAsync(id);
            if (alunoUc == null)
            {
                return NotFound();
            }
            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Nome", alunoUc.IdAluno);
            ViewData["IdUc"] = new SelectList(_context.Ucs, "Id", "Nome", alunoUc.IdUc);
            return View(alunoUc);
        }

        // POST: AlunoUcs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUc,IdCurso,IdAluno")] AlunoUc alunoUc)
        {
            if (id != alunoUc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alunoUc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoUcExists(alunoUc.Id))
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
            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Nome", alunoUc.IdAluno);
            ViewData["IdUc"] = new SelectList(_context.Ucs, "Id", "Nome", alunoUc.IdUc);
            return View(alunoUc);
        }

        // GET: AlunoUcs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alunoUc = await _context.AlunoUcs
                .Include(a => a.IdAlunoNavigation)
                .Include(a => a.IdUcNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alunoUc == null)
            {
                return NotFound();
            }

            return View(alunoUc);
        }

        // POST: AlunoUcs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alunoUc = await _context.AlunoUcs.FindAsync(id);
            if (alunoUc != null)
            {
                _context.AlunoUcs.Remove(alunoUc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoUcExists(int id)
        {
            return _context.AlunoUcs.Any(e => e.Id == id);
        }
    }
}
