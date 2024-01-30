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


        public async Task<IActionResult> Index(int? aulaId)
        {
            // Recebe todas a presencas e filtra por id se for o caso
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
            // 1. Verifica se o ID na URL corresponde ao ID da presença
            if (id != presenca.Id)
            {
                return NotFound(); // Retorna NotFound se não houver correspondência
            }

            // 2. Verifica se o modelo é válido
            if (ModelState.IsValid)
            {
                try
                {
                    // 3. Actualização via API
                    var apiClient = _httpClientFactory.CreateClient();

                    // Cria um DTO para atualizar apenas a propriedade 'Presente'
                    var presencaUpdateDto = new PresencaDTO { Presente = presenca.Presente };
                    var presencaJson = JsonConvert.SerializeObject(presencaUpdateDto);
                    var content = new StringContent(presencaJson, Encoding.UTF8, "application/json");

                    // Faz uma requisição PUT para a API de Presenças
                    var response = await apiClient.PutAsync($"http://localhost:5031/api/presencas/{id}", content);

                    // Trata o caso em que a API request falha
                    if (!response.IsSuccessStatusCode)
                    {
                        return View("Error");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Trata exceções de concorrência
                    if (!PresencaExists(presenca.Id))
                    {
                        return NotFound(); // Retorna NotFound se a presença não existe
                    }
                    else
                    {
                        throw;
                    }
                }

                // 4. Redireciona para a página de Presenças com um parâmetro de identificador de aula
                var redirectUrl = $"/Presencas?aulaId={presenca.IdAula}";
                return Redirect(redirectUrl);
            }

            // 5. Fornece dados para as listas suspensas na View
            ViewData["IdAluno"] = new SelectList(_context.Alunos, "Id", "Id", presenca.IdAluno);
            ViewData["IdAula"] = new SelectList(_context.Aulas, "Id", "Id", presenca.IdAula);

            // 6. Retorna a View com a presença (para correção se houver erros no modelo)
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
