using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
    public partial class MainWindow : Window
    {
        private String ksData = "";

        private List<MediaData> tLItems = new List<MediaData>();
        private List<MediaData> tCItems = new List<MediaData>();
        private List<MediaData> tRItems = new List<MediaData>();

        private List<MediaData> cLItems = new List<MediaData>();
        private List<MediaData> cCItems = new List<MediaData>();
        private List<MediaData> cRItems = new List<MediaData>();

        private List<MediaData> bLItems = new List<MediaData>();
        private List<MediaData> bCItems = new List<MediaData>();
        private List<MediaData> bRItems = new List<MediaData>();

        private Marquee MarqueeItem = new Marquee();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //tlMedia.Source = new Uri("C:\\Users\\Ascii\\Downloads\\12.gif", UriKind.Absolute);
            //imgMedia.Source = new BitmapImage(new Uri("C:\\Users\\Ascii\\Downloads\\12.gif", UriKind.Absolute));
            String path = System.AppDomain.CurrentDomain.BaseDirectory;
            String fullFileName = System.AppDomain.CurrentDomain.FriendlyName;
            int dotIndex = fullFileName.IndexOf('.');
            String fileName = fullFileName.Substring(0, dotIndex);
            String ksdFileName = String.Format("{0}{1}.ksd", path, fileName);

            if (File.Exists(ksdFileName))
            {
                String fileContent = File.ReadAllText(ksdFileName, Encoding.Unicode);
                byte[] encContentBytes = Convert.FromBase64String(fileContent);
                RC4 rc4Obj = new RC4("a555ab555bc555cd555d");
                rc4Obj.EncryptInPlace(encContentBytes);
                ksData = getString(encContentBytes);
            }
            else
            {
                MessageBox.Show(String.Format("同層資料夾中找不到 {0}.ksd 檔案", fileName));
            }
        }

        private String getString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
