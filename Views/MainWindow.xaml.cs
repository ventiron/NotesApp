using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NotesApp.Interfaces;


namespace NotesApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnClose;
        }

        public void OnClose(object sender, RoutedEventArgs e)
        {
            if(DataContext is IClosable vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };

                Closing += (s, e) =>
                {
                    e.Cancel = !vm.CanClose();
                };
            }
        }
            














        ListBox dragSource;

        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }


        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            object dataN = GetDataFromListBox(parent, e.GetPosition(parent));

            var dropList = ((IList)parent.ItemsSource);
            var originalList = ((IList)dragSource.ItemsSource);

            if (dataN == null)
            {
                object data = e.Data.GetData(originalList.GetType().GetGenericArguments()[0]);
                dropList.Add(data);
                originalList.Remove(data);
            }
            else
            {
                int index = dropList.IndexOf(dataN);
                object data = e.Data.GetData(originalList.GetType().GetGenericArguments()[0]);
                originalList.Remove(data);
                dropList.Insert(index, data);
            }
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount > 1)
            {
                ListBox parent = (ListBox)sender;
                dragSource = parent;
                object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

                if (data != null)
                {
                    DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
                }
            }
        }
    }
}
