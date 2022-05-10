using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace NotesApp.Models
{
    internal class Note : INotifyPropertyChanged
    {
        public Note(int Id, string Name, string Text)
        {
            this.Date = DateTime.Now;
            this.Id = Id;
            this.Name = Name;
            this.Text = Text;
        }
        private DateTime date { get; set; }
        private int id { get; set; }
        private string name { get; set; }
        private string text { get; set; }


        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChange("Date");
            }
        }
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChange("Id");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChange("Name");
            }
        }
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChange("Text");
            }
        }
        public Group group { get; set; }
        public int GroupId { get; set; }
        public string BreaklessText { get
            {
                return Text.Replace(Environment.NewLine, " ");
            } 
        }

        public Note(string Name, string Text)
        {
            this.date = DateTime.Now;
            this.Name = Name;
            this.Text = Text;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
