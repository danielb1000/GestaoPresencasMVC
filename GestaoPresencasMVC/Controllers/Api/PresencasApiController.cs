using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoPresencasMVC.Models;
using GestaoPresencasMVC.DTOs;

namespace GestaoPresencasMVC.Controllers.Api
{
    [Route("api/presencas")]
    [ApiController]
    public class PresencasApiController : ControllerBase
    {
        private readonly TentativaDb4Context _context;

        public PresencasApiController(TentativaDb4Context context)
        {
            _context = context;
        }

        // GET: api/Presencas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Presenca>>> GetPresencas()
        {
            Console.WriteLine("\n\n\nGET PRESENCAS API METHOD CALLED\n\n\n");
            return await _context.Presencas.ToListAsync();
        }

        // GET: api/Presencas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Presenca>> GetPresenca(int id)
        {
            var presenca = await _context.Presencas.FindAsync(id);
            Console.WriteLine("\n\n\nGET PRESENCAS API METHOD CALLED\n\n\n");
            if (presenca == null)
            {
                return NotFound();
            }

            return presenca;
        }

        // PUT: api/PresencasApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePresenca(int id, [FromBody] PresencaDTO presencaUpdateDto)
        {
            Console.WriteLine("\n\n\nPUT PRESENCAS API METHOD CALLED\n\n\n");
            if (presencaUpdateDto == null)
            {
                return BadRequest("Invalid data");
            }

            // Retrieve the existing presenca
            var presenca = await _context.Presencas.FindAsync(id);

            if (presenca == null)
            {
                return NotFound();
            }

            // Update the 'Presente' property
            presenca.Presente = presencaUpdateDto.Presente;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(presenca);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "Internal Server Error");
            }


            return NoContent();
        }


        // POST: api/Presencas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Presenca>> PostPresenca(Presenca presenca)
        {
            _context.Presencas.Add(presenca);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPresenca", new { id = presenca.Id }, presenca);
        }

        // DELETE: api/Presencas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePresenca(int id)
        {
            var presenca = await _context.Presencas.FindAsync(id);
            if (presenca == null)
            {
                return NotFound();
            }

            _context.Presencas.Remove(presenca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PresencaExists(int id)
        {
            return _context.Presencas.Any(e => e.Id == id);
        }
    }
}
