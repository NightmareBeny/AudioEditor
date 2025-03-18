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

namespace Обрезка_аудио
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isPlay = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Change the volume of the media.
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            MediaAudio.Volume = (double)volumeSlider.Value;
        }

        // When the media opens, initialize the "Seek To" slider maximum value
        // to the total number of miliseconds in the length of the media clip.
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            if (MediaAudio.NaturalDuration.HasTimeSpan)
                timelineSlider.Maximum = MediaAudio.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        // When the media playback is finished. Stop() the media to seek to media start.
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            MediaAudio.Stop();
        }

        // Jump to different parts of the media (seek to).
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)timelineSlider.Value;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            MediaAudio.Position = ts;
        }

        void InitializePropertyValues()
        {
            // Set the media's starting Volume and SpeedRatio to the current value of the
            // their respective slider controls.
            MediaAudio.Volume = (double)volumeSlider.Value;
        }

        private void PlayPauseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            PlayPauseButton.Background = Brushes.LawnGreen;
        }

        private void PlayPauseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            PlayPauseButton.Background = Brushes.LimeGreen;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isPlay)
            {
                // Play the media
                MediaAudio.Play();
                isPlay = true;
            }
            else
            {
                // Pause the media.
                MediaAudio.Pause();
                isPlay= false;
            }
            // Initialize the MediaElement property values.
            InitializePropertyValues();
        }

        private void StopButton_MouseEnter(object sender, MouseEventArgs e)
        {
            StopButton.Background = Brushes.Red;
        }

        private void StopButton_MouseLeave(object sender, MouseEventArgs e)
        {
            StopButton.Background = Brushes.Crimson;
        }

        // Stop the media
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            MediaAudio.Stop();
            isPlay = false;
        }
    }
}