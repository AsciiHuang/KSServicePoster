using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KSServicePoster
{
    public class Marquee : INotifyPropertyChanged
    {

        private String content = "";
        public String Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                NotifyPropertyChanged("Content");
            }
        }

        private String background ="#000000";
        public String Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
                NotifyPropertyChanged("Background");
            }
        }

        private String fontColor = "#FFFFFF";
        public String FontColor
        {
            get
            {
                return fontColor;
            }
            set
            {
                fontColor = value;
                NotifyPropertyChanged("FontColor");
            }
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
