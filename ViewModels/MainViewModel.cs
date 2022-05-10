using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using NotesApp.Models;
using System.Collections.Specialized;
using System.Windows;
using NotesApp.Entity;
using Microsoft.EntityFrameworkCore;
using NotesApp.Commands;
using NotesApp.Interfaces;

namespace NotesApp.ViewModels
{
    internal class MainViewModel : BaseViewModel, IClosable
    {
        NoteContext noteContext = new NoteContext();

        public ObservableCollection<Group> groups { get; private set; }

        private Note selectedNote;
        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                
                selectedNote = value;
                OnPropertyChange("SelectedNote");
            }
        }

        public MainViewModel()
        {
            groups = new ObservableCollection<Group>(noteContext.groups.Include(gr=>gr.Notes));
        }





        private SimpleCommand save_Command;
        public SimpleCommand Save_Command
        {
            get
            {
                return save_Command ?? (save_Command = new SimpleCommand(obj =>
                {
                    noteContext.SaveChanges();
                }));
            }
        }
        private SimpleCommand addGroup_Command;
        public SimpleCommand AddGroup_Command
        {
            get
            {
                return addGroup_Command ?? (addGroup_Command = new SimpleCommand(obj =>
                 {
                     var group = new Group("Новая группа");
                     this.groups.Add(group);
                     noteContext.groups.Add(group);
                     noteContext.SaveChanges();
                 }));

            }
        }


        private SimpleCommand addNote_Command;
        public SimpleCommand AddNote_Command
        {
            get
            {
                return addNote_Command ?? (addNote_Command = new SimpleCommand(obj =>
                {
                    var group = new Note("Новая запись","Текст");
                    this.groups[groups.IndexOf(obj as Group)].Notes.Add(group);
                    noteContext.notes.Add(group);
                    noteContext.SaveChanges();
                }));

            }
        }

        private SimpleCommand deleteGroup_Command;
        public SimpleCommand DeleteGroup_Command
        {
            get
            {
                return deleteGroup_Command ?? (deleteGroup_Command = new SimpleCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить данную группу заметок? Все заметки внутри так же будут удалены",
                        "Удаление группы заметок.", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            this.groups.Remove(obj as Group);
                            noteContext.groups.Remove(obj as Group);
                            noteContext.SaveChanges();
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }));

            }
        }
        private SimpleCommand deleteNote_Command;
        public SimpleCommand DeleteNote_Command
        {
            get
            {
                return deleteNote_Command ?? (deleteNote_Command = new SimpleCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить данную заметку?",
                        "Удаление группы заметок.", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Note note = obj as Note;
                            Group group = (from g in groups where g.Notes.Contains(note) select g).First();
                            group.Notes.Remove(note);
                            noteContext.notes.Remove(obj as Note);
                            noteContext.SaveChanges();
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }));

            }
        }


        public void change(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
            };
        }




        public Action Close { get; set; }
        public bool CanClose()
        {
            if (noteContext.ChangeTracker.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Присутствуют несохраненные изменениея. Хотите их сохранить?", "Сохранение при закрытии",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        noteContext.SaveChanges();
                        return true;
                    case MessageBoxResult.No:
                        return true;
                    case MessageBoxResult.Cancel:
                        return false;
                }
            }
            return true;
        }

    }



}
