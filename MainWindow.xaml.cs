using NAudio.Wave;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Обрезка_аудио
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlay = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        // When the media opens, initialize the "Seek To" slider maximum value
        // to the total number of miliseconds in the length of the media clip.
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            if (mediaAudio.NaturalDuration.HasTimeSpan)
            {
                timelineSlider.Maximum = mediaAudio.NaturalDuration.TimeSpan.TotalMilliseconds;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += TimerTick;
                timer.Start();
            }
        }

        // When the media playback is finished. Stop() the media to seek to media start.
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            mediaAudio.Stop();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if ((mediaAudio.Source != null) && (mediaAudio.NaturalDuration.HasTimeSpan))
            {
                timelineSlider.Value = mediaAudio.Position.TotalMilliseconds;
            }
        }

        // Jump to different parts of the media (seek to).
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)timelineSlider.Value;
            mediaAudio.Position = TimeSpan.FromSeconds(timelineSlider.Value);
            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            var ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            mediaAudio.Position = ts;
            timeLine.Visibility = Visibility.Visible;
            timeLine.Text = TimeSpan.FromSeconds(ts.TotalSeconds).ToString(@"hh\:mm\:ss");
            duration.Text = $" - {TimeSpan.FromSeconds(mediaAudio.NaturalDuration.TimeSpan.TotalSeconds):hh\\:mm\\:ss}";
        }

        // Change the volume of the media.
        private void ChangeMediavolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaAudio.Volume = (double)volumeSlider.Value;
        }

        void InitializePropertyValues()
        {
            // Set the media's starting volume to the current value of the their respective slider controls.
            mediaAudio.Volume = (double)volumeSlider.Value;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isPlay)
            {
                // Play the media
                mediaAudio.Play();
                isPlay = true;
            }
            else
            {
                // Pause the media.
                mediaAudio.Pause();
                isPlay= false;
            }
            // Initialize the MediaElement property values.
            InitializePropertyValues();
        }

        // Stop the media
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaAudio.Stop();
            isPlay = false;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double xChange = 1, yChange = 1;
            if (e.PreviousSize.Width != 0)
                xChange = e.NewSize.Width / e.PreviousSize.Width;
            if (e.PreviousSize.Height != 0)
                yChange = (e.NewSize.Height / e.PreviousSize.Height);

            volume.FontSize *= xChange;
            time.FontSize *= xChange;

            stopButton.Width *= xChange;
            stopButton.Height *= yChange;
            playPauseButton.Width *= xChange;
            playPauseButton.Height *= yChange;
            if (xChange>1 && yChange>1)
                volumeSlider.RenderTransform = new ScaleTransform(1.9, 1.9);
            else volumeSlider.RenderTransform = new ScaleTransform(1, 1);
        }
    }
}