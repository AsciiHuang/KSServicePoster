using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace KSServicePoster
{
    /// <summary>
    /// Interaction logic for Poster.xaml
    /// </summary>
    public partial class Poster : Window
    {
        private List<String> dataContents = new List<String>();

        public Poster()
        {
            InitializeComponent();
        }

        public Poster(List<String> items)
        {
            dataContents.Clear();
            for (int i = 0; i < items.Count; ++i)
            {
                dataContents.Add(items[i]);
            }
        }
    }
}
