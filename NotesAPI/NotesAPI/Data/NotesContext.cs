using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;
using System.Collections.Generic;

namespace NotesAPI.Data
{
    public class NotesContext : DbContext
    {
        public NotesContext(DbContextOptions<NotesContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }
    }
}
