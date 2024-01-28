using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoPresencasMVC.Models;

namespace GestaoPresencasMVC.Controllers.Api
{
    [Route("api/aulas")]
    [ApiController]
    public class AulasApiController : ControllerBase
    {
        private readonly TentativaDb4Context _context;

        public AulasApiController(TentativaDb4Context context)
        {
            _context = context;
        }

        // GET: api/Aulas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aula>>> GetAulas()
        {
            Console.WriteLine("GetAulas API method called!\nGetAulas API method called!\nGetAulas API method called!\nGetAulas API method called!\n");
            var aulas = await _context.Aulas
                .Include(a => a.IdAnoNavigation)
                .Include(a => a.IdUcNavigation)
                .ToListAsync();

            return aulas;
        }

        // GET: api/Aulas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aula>> GetAula(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);

            if (aula == null)
            {
                return NotFound();
            }

            return aula;
        }

        // PUT: api/Aulas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAula(int id, Aula aula)
        {
            if (id != aula.Id)
            {
                return BadRequest();
            }

            _context.Entry(aula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AulaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<Aula>> PostAula(Aula aula)
        {
            // Step 1: Save the Aula
            _context.Aulas.Add(aula);
            await _context.SaveChangesAsync();

            // Step 2: Retrieve the list of alunos in the associated UC
            List<int> alunosIds = _context.AlunoUcs
                .Where(au => au.IdUc == aula.IdUc)
                .Select(au => au.IdAluno ?? 0)
                .ToList();

            // Step 3: Create Presenca records for each Aluno
            foreach (int alunoId in alunosIds)
            {
                Presenca presenca = new Presenca
                {
                    IdAula = aula.Id,
                    IdAluno = alunoId,
                    Presente = false // You may set the default value based on your logic
                };

                _context.Presencas.Add(presenca);
            }

            await _context.SaveChangesAsync();

            // Return the created Aula
            return CreatedAtAction(nameof(GetAula), new { id = aula.Id }, aula);
        }


        private bool AulaExists(int id)
        {
            return _context.Aulas.Any(e => e.Id == id);
        }
    }
}
