using NAudio.Wave;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
            leftTimeLine.Text = "00:00:00";
            rightTimeLine.Text = "00:00:00";
        }

        // When the media opens, initialize the timelineSlider maximum value to the total number of miliseconds in the length of the media clip.
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            if (mediaAudio.NaturalDuration.HasTimeSpan)
            {
                timelineSlider.Maximum = mediaAudio.NaturalDuration.TimeSpan.TotalMilliseconds;
                timelineSlider.RightValue = timelineSlider.Maximum;
                timelineSlider.Minimum = 0;
                timelineSlider.LeftValue = timelineSlider.Minimum;
                leftTimeLine.Text = TimeSpan.FromMilliseconds(timelineSlider.LeftValue).ToString(@"hh\:mm\:ss");
                rightTimeLine.Text = TimeSpan.FromMilliseconds(timelineSlider.RightValue).ToString(@"hh\:mm\:ss");
                duration.Text = $" (Общее время: {TimeSpan.FromSeconds(mediaAudio.NaturalDuration.TimeSpan.TotalSeconds):hh\\:mm\\:ss})";
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
            if (mediaAudio.Source != null && mediaAudio.NaturalDuration.HasTimeSpan && isPlay)
            {
                timelineSlider.MiddleValue = mediaAudio.Position.TotalMilliseconds;
            }
        }

        // Jump to different parts of the media (seek to).
        private void timelineSlider_MiddleValueChanged(object sender, EventArgs e)
        {
            if (timelineSlider.MiddleValue < timelineSlider.RightValue)
            {
                mediaAudio.Position = TimeSpan.FromMilliseconds(timelineSlider.MiddleValue);
                leftTimeLine.Text = TimeSpan.FromMilliseconds(timelineSlider.MiddleValue).ToString(@"hh\:mm\:ss");
            }
            else timelineSlider_LeftValueChanged(sender, e);
        }

        private void timelineSlider_LeftValueChanged(object sender, EventArgs e)
        {
            mediaAudio.Pause();
            leftTimeLine.Text = TimeSpan.FromMilliseconds(timelineSlider.LeftValue).ToString(@"hh\:mm\:ss");
            timelineSlider.MiddleValue = timelineSlider.LeftValue;
            mediaAudio.Position = TimeSpan.FromMilliseconds(timelineSlider.MiddleValue);
            mediaAudio.Play();
        }

        private void timelineSlider_RightValueChanged(object sender, EventArgs e)
        {
            rightTimeLine.Text = TimeSpan.FromMilliseconds(timelineSlider.RightValue).ToString(@"hh\:mm\:ss");
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
                isPlay = false;
            }
            // Initialize the MediaElement property values.
            InitializePropertyValues();
        }

        // Stop the media
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaAudio.Pause();
            isPlay = false;
            timelineSlider.MiddleValue = timelineSlider.LeftValue;
            mediaAudio.Position = TimeSpan.FromMilliseconds(timelineSlider.MiddleValue);
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
            if (xChange > 1 && yChange > 1)
                volumeSlider.RenderTransform = new ScaleTransform(1.9, 1.9);
            else volumeSlider.RenderTransform = new ScaleTransform(1, 1);
        }
    }
}