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
using GestaoPresencasMVC.DTOs;

namespace GestaoPresencasMVC.Controllers
{
    public class AulasController : BaseController
    {
        private readonly TentativaDb4Context _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<gpUser> _userManager;

        public AulasController(TentativaDb4Context context, UserManager<gpUser> userManager, IHttpClientFactory httpClientFactory)
             : base(userManager)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }

        // GET: Aulas
        public async Task<IActionResult> Index()
        {
            // Call the API to get the list of Aulas with presenca counts
            var apiClient = _httpClientFactory.CreateClient();
            var response = await apiClient.GetStringAsync("http://localhost:5031/api/aulas/GetAulasWithPresencaCount");
            var aulasWithPresencaCount = JsonConvert.DeserializeObject<List<AulaWithPresencaCountDTO>>(response);

            return View(aulasWithPresencaCount); // Pass the correct model to the view
        }



        // GET: Aulas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Make a request to the API to get details for the specified Aula Id
            var apiClient = _httpClientFactory.CreateClient();
            var response = await apiClient.GetStringAsync($"http://localhost:5031/api/aulas/{id}");

            if (string.IsNullOrEmpty(response))
            {
                return NotFound();
            }

            // Deserialize the response to an Aula object
            var aula = JsonConvert.DeserializeObject<Aula>(response);

            if (aula == null)
            {
                return NotFound();
            }

            // Pass the Aula Id to the PresencasController
            return RedirectToAction("Index", "Presencas", new { aulaId = aula.Id });
        }


        // GET: Aulas/Create
        public async Task<IActionResult> Create()
        {
            // the logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // se nao estiver autenticado
                return Redirect($"/Identity/Account/Login");
            }
        
            int? docenteId = user.DocenteId;

            if (docenteId == null)
            {
                // se nao for docente
                return Redirect($"/Identity/Account/Login");
            }

            // Ucs do docente
            var ucsForDocente = await _context.Ucs
                .Where(u => u.IdDocente == docenteId)
                .ToListAsync();

            // SelectList para as UCs do docente
            ViewData["IdUc"] = new SelectList(ucsForDocente, "Id", "Nome");
            ViewData["IdAno"] = new SelectList(_context.Anos, "Id", "Numero");

            return View();
        }



        // POST: Aulas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUc,IdAno,Data,Sala")] Aula aula)
        {
            // Passo 1: Verifica se o estado do modelo é válido
            if (ModelState.IsValid)
            {
                // Passo 2: Faz uma solicitação POST para a API para criar a Aula
                var apiClient = _httpClientFactory.CreateClient();

                // Serializa o objeto Aula para JSON
                var aulaJson = JsonConvert.SerializeObject(aula);

                // Cria o conteúdo para a solicitação HTTP
                var content = new StringContent(aulaJson, Encoding.UTF8, "application/json");

                // Envia uma solicitação POST para a API de Aulas
                var response = await apiClient.PostAsync("http://localhost:5031/api/aulas", content);

                // Verifica se a solicitação à API foi bem-sucedida
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error"); // Retorna uma view/vista de erro se a solicitação à API falhar
                }

                // Redireciona para a ação Index após a criação bem-sucedida
                return RedirectToAction(nameof(Index));
            }

            // Passo 3: Retorna a View com o objeto Aula se o estado do modelo não for válido
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
        public async Task<IActionResult> DeleteAula(int id)
        {
            // Find the Aula along with its related Presenca records
            var aula = await _context.Aulas
                .Include(a => a.Presencas)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aula == null)
            {
                return NotFound();
            }

            // Remove the related Presenca records
            _context.Presencas.RemoveRange(aula.Presencas);

            // Remove the Aula
            _context.Aulas.Remove(aula);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool AulaExists(int id)
        {
            return _context.Aulas.Any(e => e.Id == id);
        }
    }
}