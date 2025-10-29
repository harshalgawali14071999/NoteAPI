using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Data;
using NotesAPI.Models;

namespace NotesAPI.Controllers
{
    [Route("api/Notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesContext _context;

        public NotesController(NotesContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Note note)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            note.CreatedAt = DateTime.UtcNow;
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        // GET /notes
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        {
            var query = _context.Notes.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(n => n.Title.Contains(keyword) ||
                                         (n.Content ?? "").Contains(keyword));
            }

            var notes = await query.ToListAsync();
            return Ok(notes);
        }

        // GET /notes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
                return NotFound(new { message = "Note not found." });

            return Ok(note);
        }

        // PUT /notes/{id} (Optional)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Note updatedNote)
        {
            var existing = await _context.Notes.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Note not found." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existing.Title = updatedNote.Title;
            existing.Content = updatedNote.Content;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // DELETE /notes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
                return NotFound(new { message = "Note not found." });

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Note deleted successfully." });
        }
    }
}
