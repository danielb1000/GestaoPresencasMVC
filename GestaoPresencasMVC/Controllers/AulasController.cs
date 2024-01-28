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
using Newtonsoft.Json;
using System.Text;

namespace GestaoPresencasMVC.Controllers
{
    public class AulasController : BaseController
    {
        private readonly TentativaDb4Context _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public AulasController(TentativaDb4Context context, UserManager<gpUser> userManager, IHttpClientFactory httpClientFactory)
             : base(userManager)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Aulas
        public async Task<IActionResult> Index()
        {
            // Call the API to get the list of Aulas
            var apiClient = _httpClientFactory.CreateClient();
            var response = await apiClient.GetStringAsync("http://localhost:5031/api/aulas");
            var aulas = JsonConvert.DeserializeObject<List<Aula>>(response);

            return View(aulas);
        }

        // GET: Aulas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas
                .Include(a => a.IdAnoNavigation)
                .Include(a => a.IdUcNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aula == null)
            {
                return NotFound();
            }

            // Pass the Aula Id to the PresencasController
            return RedirectToAction("Index", "Presencas", new { aulaId = aula.Id });
        }


        // GET: Aulas/Create
        public IActionResult Create()
        {
            ViewData["IdAno"] = new SelectList(_context.Anos, "Id", "Numero");
            ViewData["IdUc"] = new SelectList(_context.Ucs, "Id", "Nome");
            return View();
        }

        // POST: Aulas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,IdUc,IdAno,Data,Sala")] Aula aula)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(aula);
        //        await _context.SaveChangesAsync();

        //        // Retrieve the list of alunos in the associated UC
        //        List<int> alunosIds = _context.AlunoUcs
        //            .Where(au => au.IdUc == aula.IdUc)
        //            .Select(au => au.IdAluno ?? 0)
        //            .ToList();

        //        // Create Presenca records for each Aluno
        //        foreach (int alunoId in alunosIds)
        //        {
        //            Presenca presenca = new Presenca
        //            {
        //                IdAula = aula.Id,
        //                IdAluno = alunoId,
        //                Presente = false // You may set the default value based on your logic
        //            };

        //            _context.Presencas.Add(presenca);
        //        }

        //        await _context.SaveChangesAsync();

        //        return RedirectToAction(nameof(Index));
        //    }

        //    // Handle invalid model state
        //    ViewData["IdAno"] = new SelectList(_context.Anos, "Id", "Id", aula.IdAno);
        //    ViewData["IdUc"] = new SelectList(_context.Ucs, "Id", "Id", aula.IdUc);
        //    return View(aula);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUc,IdAno,Data,Sala")] Aula aula)
        {
            if (ModelState.IsValid)
            {
                // Step 1: Make a POST request to the API to create the Aula
                var apiClient = _httpClientFactory.CreateClient();

                var aulaJson = JsonConvert.SerializeObject(aula);
                var content = new StringContent(aulaJson, Encoding.UTF8, "application/json");

                var response = await apiClient.PostAsync("http://localhost:5031/api/aulas", content);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the case where the API request fails
                    // You may want to log the error or return an error view
                    return View("Error");
                }

                return RedirectToAction(nameof(Index));
            }

            // Handle invalid model state
            ViewData["IdAno"] = new SelectList(_context.Anos, "Id", "Id", aula.IdAno);
            ViewData["IdUc"] = new SelectList(_context.Ucs, "Id", "Id", aula.IdUc);
            return View(aula);
        }






        // GET: Aulas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas.FindAsync(id);
            if (aula == null)
            {
                return NotFound();
            }
            ViewData["IdAno"] = new SelectList(_context.Anos, "Id", "Numero", aula.IdAno);
            ViewData["IdUc"] = new SelectList(_context.Ucs, "Id", "Nome", aula.IdUc);
            return View(aula);
        }

        // POST: Aulas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUc,IdAno,Data,Sala")] Aula aula)
        {
            if (id != aula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AulaExists(aula.Id))
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
            ViewData["IdAno"] = new SelectList(_context.Anos, "Id", "Id", aula.IdAno);
            ViewData["IdUc"] = new SelectList(_context.Ucs, "Id", "Id", aula.IdUc);
            return View(aula);
        }

        // GET: Aulas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas
                .Include(a => a.IdAnoNavigation)
                .Include(a => a.IdUcNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aula == null)
            {
                return NotFound();
            }

            return View(aula);
        }

        // POST: Aulas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the Aula along with its related Presenca records
            var aula = await _context.Aulas
                .Include(a => a.Presencas)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aula != null)
            {
                // Remove the related Presenca records
                _context.Presencas.RemoveRange(aula.Presencas);

                // Remove the Aula
                _context.Aulas.Remove(aula);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool AulaExists(int id)
        {
            return _context.Aulas.Any(e => e.Id == id);
        }
    }
}
