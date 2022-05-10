using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace NotesApp.Entity
{
    internal class NoteContext : DbContext
    {
        public DbSet<Note> notes => Set<Note>();
        public DbSet<Group> groups => Set<Group>();


        private string _currentFolderPath 
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }
        private string _fullDBFilePath
        {
            get
            {
                return System.IO.Path.Combine(_currentFolderPath, "DbFile.db");
            }
        }

        private string _debugDBPath
        {
            get
            {
                return "D:/DB/sec.db";
            }
        }

        public NoteContext()
        {
            Database.EnsureCreated();
        }
        public NoteContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
                options.UseSqlite($"Data Source={_fullDBFilePath}");
        }

    }
}
