using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSServicePoster
{
    public class MediaData : INotifyPropertyChanged
    {
        private Constants.MediaType type = Constants.MediaType.None;
        public Constants.MediaType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
            }
        }

        private String path = "";
        public String Path
        {
            get
            {
                String resStr = "點選編輯新的 圖片 或 影片參數";

                if (!String.IsNullOrEmpty(path))
                {
                    resStr = path;
                }

                return resStr;
            }
            set
            {
                path = value;
                NotifyPropertyChanged("Path");
            }
        }

        private String internalPath = "";
        public String InternalPath
        {
            get
            {
                return internalPath;
            }
            set
            {
                internalPath = value;
                NotifyPropertyChanged("InternalPath");
            }
        }

        private Constants.MediaDuration duration = Constants.MediaDuration.None;
        public Constants.MediaDuration Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                NotifyPropertyChanged("Duration");
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
