using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace KSServicePoster
{
    public class MediaPanel : Grid
    {
        public MediaPanel()
        {
            VideoMedia.Stretch = Stretch.Fill;
            Children.Add(VideoMedia);
        }

        private int currentIndex = 0;
        private List<MediaData> mediaItems = new List<MediaData>();
        private DispatcherTimer timer = null;

        private MediaElement VideoMedia = new MediaElement();

        public void setData(List<MediaData> items)
        {
            mediaItems.Clear();
            for (int i = 0; i < items.Count; ++i)
            {
                mediaItems.Add(items[i]);
            }

            refreshMediaData();
        }

        private void refreshMediaData()
        {
            VideoMedia.MediaEnded -= OnVideoMediaPlayEnded;
            if (timer != null)
            {
                timer.Stop();
            }
            MediaData currentMediaData = mediaItems[currentIndex];
            if (currentMediaData.Type == Constants.MediaType.Photo)
            {
                if (currentMediaData.Path.ToLower().EndsWith(".gif"))
                {
                    VideoMedia.MediaEnded += OnVideoMediaPlayEnded;
                    VideoMedia.Source = new Uri(currentMediaData.Path, UriKind.Relative);
                }
                else
                {
                    VideoMedia.Source = new Uri(currentMediaData.Path, UriKind.Relative);
                    timer = new DispatcherTimer();
                    timer.Interval = getTimeSpan(currentMediaData.Duration);
                    timer.Tick += OnTimerTick;
                    timer.Start();
                }
            }
            else if (currentMediaData.Type == Constants.MediaType.Video)
            {
                VideoMedia.MediaEnded += OnVideoMediaPlayEnded;
                VideoMedia.Source = new Uri(currentMediaData.Path, UriKind.Relative);
            }
            else
            {
                // set to null and wait 5 seconds
                VideoMedia.Source = null;
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(5);
                timer.Tick += OnTimerTick;
                timer.Start();
            }

            ++currentIndex;
            if (currentIndex >= mediaItems.Count)
            {
                currentIndex = 0;
            }
        }

        private TimeSpan getTimeSpan(Constants.MediaDuration duration)
        {
            
            if (duration == Constants.MediaDuration.Second5)
            {
                return TimeSpan.FromSeconds(5);
            }
            else if (duration == Constants.MediaDuration.Second10)
            {
                return TimeSpan.FromSeconds(10);
            }
            else if (duration == Constants.MediaDuration.Second15)
            {
                return TimeSpan.FromSeconds(15);
            }
            else if (duration == Constants.MediaDuration.Second20)
            {
                return TimeSpan.FromSeconds(20);
            }
            else if (duration == Constants.MediaDuration.Second25)
            {
                return TimeSpan.FromSeconds(25);
            }
            else if (duration == Constants.MediaDuration.Second30)
            {
                return TimeSpan.FromSeconds(30);
            }
            else if (duration == Constants.MediaDuration.Second35)
            {
                return TimeSpan.FromSeconds(35);
            }
            else if (duration == Constants.MediaDuration.Second40)
            {
                return TimeSpan.FromSeconds(40);
            }
            else if (duration == Constants.MediaDuration.Second45)
            {
                return TimeSpan.FromSeconds(45);
            }
            else if (duration == Constants.MediaDuration.Second50)
            {
                return TimeSpan.FromSeconds(50);
            }
            else if (duration == Constants.MediaDuration.Second55)
            {
                return TimeSpan.FromSeconds(55);
            }
            else if (duration == Constants.MediaDuration.Second60)
            {
                return TimeSpan.FromSeconds(60);
            }
            else if (duration == Constants.MediaDuration.Unlimited)
            {
                return TimeSpan.FromSeconds(-1);
            }
            else
            {
                return TimeSpan.FromSeconds(1);
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            timer.Stop();
            refreshMediaData();
        }

        private void OnVideoMediaPlayEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            refreshMediaData();
        }

    }
}
