using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NotesApp.Models
{
    internal class Group
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ObservableCollection<Note> Notes { get; set; }

        public Group(string Title)
        {
            this.Title = Title;
            Notes = new ObservableCollection<Note>();
        }
    }
}
