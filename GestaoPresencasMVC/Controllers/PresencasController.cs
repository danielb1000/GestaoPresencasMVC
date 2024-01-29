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
using GestaoPresencasMVC.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace GestaoPresencasMVC.Controllers
{
    public class PresencasController : BaseController
    {
        private readonly TentativaDb4Context _context;
        private readonly IHttpClientFactory _httpClientFactory;


        public PresencasController(TentativaDb4Context context, UserManager<gpUser> userManager, IHttpClientFactory httpClientFactory)
             : base(userManager)
        {
            _context = context;
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        }

        // GET: Presencas
        //public IActionResult Index()
        //{
        //    var presencas = _context.Presencas
        //        .Include(p => p.IdAulaNavigation)
        //            .ThenInclude(aula => aula.IdUcNavigation)
        //        .Include(p => p.IdAlunoNavigation)
        //        .ToList();

        //    return View(presencas);
        //}

        // POST: Presencas/UpdatePresencas

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdatePresencas(List<Presenca> presencas)
        //{
        //    Console.WriteLine("UpdatePresencas Action Reached!");

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            foreach (var presenca in presencas)
        //            {
        //                Console.WriteLine($"Presenca Id: {presenca.Id}, Presente: {presenca.Presente}");

        //                var existingPresenca = await _context.Presencas.FindAsync(presenca.Id);

        //                if (existingPresenca != null)
        //                {
        //                    existingPresenca.Presente = presenca.Presente;
        //                    _context.Update(existingPresenca);
        //                }
        //            }

        //            await _context.SaveChangesAsync();

        //            Console.WriteLine("Changes Saved!");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Exception: {ex.Message}");
        //            throw; // Rethrow the exception after logging
        //        }


        //        return RedirectToAction(nameof(Index));
        //    }

        //    // If the ModelState is not valid, return to the same view with validation errors
        //    return View(nameof(Index), presencas);
        //}











        public async Task<IActionResult> Index(int? aulaId)
        {
            // Retrieve all presences or filter by Aula Id if provided
            IQueryable<Presenca> presencasQuery = _context.Presencas
                .Include(p => p.IdAlunoNavigation)
                .Include(p => p.IdAulaNavigation)
                    .ThenInclude(a => a.IdUcNavigation)
                .Include(p => p.IdAulaNavigation)
                    .ThenInclude(a => a.IdAnoNavigation);

            if (aulaId != null)
            {
                presencasQuery = presencasQuery.Where(p => p.IdAula == aulaId);
            }

            var presencas = await presencasQuery.ToListAsync();

            if (presencas == null)
            {
                return NotFound();
            }

            return View(presencas);
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

            var presenca = await _context.Presencas
                .Include(p => p.IdAulaNavigation)
                .Include(p => p.IdAlunoNavigation)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (presenca == null)
            {
                return NotFound();
            }

            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Nome", presenca.IdAluno);
            ViewData["IdAula"] = new SelectList(_context.Aulas, "Id", "Data", presenca.IdAula);

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
                    // Update the local database
                    _context.Update(presenca);
                    await _context.SaveChangesAsync();

                    // Make a PUT request to the Presencas API to update the 'Presente' property
                    var apiClient = _httpClientFactory.CreateClient();

                    var presencaUpdateDto = new PresencaDTO { Presente = presenca.Presente };
                    var presencaJson = JsonConvert.SerializeObject(presencaUpdateDto);
                    var content = new StringContent(presencaJson, Encoding.UTF8, "application/json");

                    var response = await apiClient.PutAsync($"http://localhost:5031/api/presencas/{id}", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        // Handle the case where the API request fails
                        // You may want to log the error or return an error view
                        return View("Error");
                    }
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
                // Redirect to the Presencas action with aulaId parameter
                var redirectUrl = $"/Presencas?aulaId={presenca.IdAula}";
                return Redirect(redirectUrl);


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
