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
            ImageMedia.Stretch = Stretch.Fill;
            VideoMedia.Stretch = Stretch.Fill;
            Children.Add(ImageMedia);
            Children.Add(VideoMedia);
        }

        private int currentIndex = 0;
        private List<MediaData> mediaItems = new List<MediaData>();
        private List<BitmapImage> bitmapImages = new List<BitmapImage>();
        private List<Uri> videos = new List<Uri>();
        private DispatcherTimer timer = null;

        private Image ImageMedia = new Image();
        private MediaElement VideoMedia = new MediaElement();

        public void setData(List<MediaData> items)
        {
            mediaItems.Clear();
            String appPath = System.AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i < items.Count; ++i)
            {
                MediaData data = items[i];
                mediaItems.Add(data);
                if (data.Type == Constants.MediaType.Photo)
                {
                    videos.Add(null);
                    bitmapImages.Add(new BitmapImage(new Uri(appPath + data.Path, UriKind.Absolute)));
                }
                else if (data.Type == Constants.MediaType.Video)
                {
                    bitmapImages.Add(null);
                    videos.Add(new Uri(data.Path, UriKind.Relative));
                }
                else
                {
                    videos.Add(null);
                    bitmapImages.Add(null);
                }
            }
            refreshMediaData();
        }

        private void refreshMediaData()
        {
            VideoMedia.Source = null;
            VideoMedia.MediaEnded -= OnVideoMediaPlayEnded;
            if (timer != null)
            {
                timer.Stop();
            }
            MediaData currentMediaData = mediaItems[currentIndex];
            if (currentMediaData.Type == Constants.MediaType.Photo && currentMediaData.Path.ToLower().EndsWith("gif"))
            {
                handleVideo(currentMediaData, false);
            }
            else if (currentMediaData.Type == Constants.MediaType.Photo)
            {
                handleImage(currentMediaData);
            }
            else if (currentMediaData.Type == Constants.MediaType.Video)
            {
                handleVideo(currentMediaData, true);
            }
            else
            {
                handleNull();
            }

            ++currentIndex;
            if (currentIndex >= mediaItems.Count)
            {
                currentIndex = 0;
            }
        }

        private void handleImage(MediaData currentMediaData)
        {
            ImageMedia.Visibility = System.Windows.Visibility.Visible;
            VideoMedia.Visibility = System.Windows.Visibility.Hidden;
            ImageMedia.Source = bitmapImages[currentIndex];
            timer = new DispatcherTimer();
            timer.Interval = getTimeSpan(currentMediaData.Duration);
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        private void handleVideo(MediaData currentMediaData, Boolean video)
        {
            ImageMedia.Visibility = System.Windows.Visibility.Hidden;
            VideoMedia.Visibility = System.Windows.Visibility.Visible;
            VideoMedia.MediaEnded += OnVideoMediaPlayEnded;
            if (video)
            {
                VideoMedia.Source = videos[currentIndex];
            }
            else
            {
                VideoMedia.Source = new Uri(currentMediaData.Path, UriKind.Relative);
            }
        }

        private void handleNull()
        {
            ImageMedia.Visibility = System.Windows.Visibility.Hidden;
            VideoMedia.Visibility = System.Windows.Visibility.Hidden;
            ImageMedia.Source = null;
            VideoMedia.Source = null;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += OnTimerTick;
            timer.Start();
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
