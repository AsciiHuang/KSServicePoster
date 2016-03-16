using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace KSServicePoster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<String> itemsSource = new ObservableCollection<String>();
        public ObservableCollection<String> ItemsSource
        {
            get
            {
                return itemsSource;
            }
            set
            {
                itemsSource = value;
                NotifyPropertyChanged("ItemsSource");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //tlMedia.Source = new Uri("C:\\Users\\Ascii\\Downloads\\12.gif", UriKind.Absolute);
            //imgMedia.Source = new BitmapImage(new Uri("C:\\Users\\Ascii\\Downloads\\12.gif", UriKind.Absolute));
        }

        private void OnAddItemButtonClicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".ksd";
            dlg.Filter = "KSService Files (*.ksd)|*.ksd";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                itemsSource.Add(dlg.FileName);
                NotifyPropertyChanged("ItemsSource");
            }
        }

        private void OnDeleteItemButtonClicked(object sender, RoutedEventArgs e)
        {
            int currentIndex = listViewContent.SelectedIndex;
            if (currentIndex != -1)
            {
                itemsSource.RemoveAt(currentIndex);
                NotifyPropertyChanged("ItemsSource");
            }
        }

        private void OnStartRunButtonClicked(object sender, RoutedEventArgs e)
        {
            new Poster(itemsSource).Show();
            this.Close();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
