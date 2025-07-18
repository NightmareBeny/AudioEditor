using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

namespace Обрезка_аудио
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlay = false;
        AudioRender audioRender = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        // When the media opens, initialize the timelineSlider maximum value to the total number of miliseconds in the length of the media clip.
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            if (mediaAudio.NaturalDuration.HasTimeSpan)
            {
                timelineSlider.Visibility = Visibility.Visible;
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
            mediaAudio.Play();
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
            if (isPlay)
            {
                mediaAudio.Pause();
                leftTimeLine.Text = TimeSpan.FromMilliseconds(timelineSlider.LeftValue).ToString(@"hh\:mm\:ss");
                timelineSlider.MiddleValue = timelineSlider.LeftValue;
                mediaAudio.Position = TimeSpan.FromMilliseconds(timelineSlider.MiddleValue);
                mediaAudio.Play();
            }
            else
            {
                leftTimeLine.Text = TimeSpan.FromMilliseconds(timelineSlider.LeftValue).ToString(@"hh\:mm\:ss");
                timelineSlider.MiddleValue = timelineSlider.LeftValue;
                mediaAudio.Position = TimeSpan.FromMilliseconds(timelineSlider.MiddleValue);
            }
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
            if (waveform.Source is not null)
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
            slash.FontSize *= xChange;
            checkboxTime.FontSize *= xChange;
            //leftTimeLine.FontSize *= xChange;
            //rightTimeLine.FontSize *= xChange;
            duration.FontSize *= xChange;

            if (xChange > 1 && yChange > 1)
            {
                volumeSlider.LayoutTransform = new ScaleTransform(1.9, 1.9);
                checkbox.LayoutTransform = new ScaleTransform(1.9, 1.9);
            }
            else
            {
                volumeSlider.LayoutTransform = new ScaleTransform(1, 1);
                checkbox.LayoutTransform = new ScaleTransform(1, 1);
            }
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            mediaAudio.Stop();
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "WAV files (*.wav)|*.wav|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.ProcessPath;
            if (openFileDialog.ShowDialog() == true)
            {
                using (var image = audioRender.MakeImageFromAudio(openFileDialog.FileName))
                {
                    // Create source
                    var myBitmapImage = new BitmapImage();
                    // BitmapImage.UriSource must be in a BeginInit/EndInit block
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = image;
                    myBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    myBitmapImage.EndInit();
                    waveform.Source = myBitmapImage;
                }
                mediaAudio.Source = new Uri(openFileDialog.FileName, UriKind.Absolute);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "WAV files (*.wav)|*.wav";
            saveFileDialog.InitialDirectory = Environment.ProcessPath;
            if (saveFileDialog.ShowDialog() == true)
            {
                var source = new AudioFileReader(audioRender.PathToAudio);
                var start = TimeSpan.FromMilliseconds(timelineSlider.LeftValue);
                var end = TimeSpan.FromMilliseconds(timelineSlider.RightValue) - start;
                if ((bool)checkbox.IsChecked)
                {
                    var nameOfFiles = new string[(int)Math.Round(end.TotalSeconds, MidpointRounding.AwayFromZero)];
                    var substring = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 4);
                    for (var i = 0; i < nameOfFiles.Length; i++) nameOfFiles[i] = substring + '_' + (i + 1) + ".wav";
                    foreach (var i in nameOfFiles)
                    {
                        source.CurrentTime = start;
                        var trimmed = source.Take(TimeSpan.FromSeconds(TimeSpan.Parse(checkboxTime.Text).TotalSeconds));
                        WaveFileWriter.CreateWaveFile16(i, trimmed);
                        start += TimeSpan.FromSeconds(TimeSpan.Parse(checkboxTime.Text).TotalSeconds);
                    }
                }
                else
                {
                    source.CurrentTime = start;
                    WaveFileWriter.CreateWaveFile16(saveFileDialog.FileName, source.Take(end));
                }
            }
        }

        private void leftTimeLine_UpValueChanged(object sender, EventArgs e)
        {
            if (TimeSpan.Parse(leftTimeLine.Text).TotalMilliseconds + TimeSpan.FromSeconds(1).TotalMilliseconds < mediaAudio.NaturalDuration.TimeSpan.TotalMilliseconds)
            {
                leftTimeLine.Text = (TimeSpan.Parse(leftTimeLine.Text) + TimeSpan.FromSeconds(1)).ToString();
                timelineSlider.LeftValue = TimeSpan.Parse(leftTimeLine.Text).TotalMilliseconds;
            }
        }

        private void leftTimeLine_DownValueChanged(object sender, EventArgs e)
        {
            if (leftTimeLine.Text != "00:00:00")
            {
                leftTimeLine.Text = (TimeSpan.Parse(leftTimeLine.Text) - TimeSpan.FromSeconds(1)).ToString();
                timelineSlider.LeftValue = TimeSpan.Parse(leftTimeLine.Text).TotalMilliseconds;
            }
        }

        private void rightTimeLine_UpValueChanged(object sender, EventArgs e)
        {
            if (TimeSpan.Parse(rightTimeLine.Text).TotalMilliseconds + TimeSpan.FromSeconds(1).TotalMilliseconds < mediaAudio.NaturalDuration.TimeSpan.TotalMilliseconds)
            {
                rightTimeLine.Text = (TimeSpan.Parse(rightTimeLine.Text) + TimeSpan.FromSeconds(1)).ToString();
                timelineSlider.RightValue = TimeSpan.Parse(rightTimeLine.Text).TotalMilliseconds;
            }
        }

        private void rightTimeLine_DownValueChanged(object sender, EventArgs e)
        {
            if (rightTimeLine.Text != "00:00:00")
            {
                rightTimeLine.Text = (TimeSpan.Parse(rightTimeLine.Text) - TimeSpan.FromSeconds(1)).ToString();
                timelineSlider.RightValue = TimeSpan.Parse(rightTimeLine.Text).TotalMilliseconds;
            }
        }

        private void checkboxTime_UpValueChanged(object sender, EventArgs e)
        {
            if (TimeSpan.Parse(checkboxTime.Text).TotalMilliseconds + TimeSpan.FromSeconds(1).TotalMilliseconds < mediaAudio.NaturalDuration.TimeSpan.TotalMilliseconds) checkboxTime.Text = (TimeSpan.Parse(checkboxTime.Text) + TimeSpan.FromSeconds(1)).ToString();
        }

        private void checkboxTime_DownValueChanged(object sender, EventArgs e)
        {
            if (checkboxTime.Text != "00:00:00") checkboxTime.Text = (TimeSpan.Parse(checkboxTime.Text) - TimeSpan.FromSeconds(1)).ToString();
        }

        private void mediaAudio_Initialized(object sender, EventArgs e)
        {
            if(timelineSlider is not null) timelineSlider.Visibility = Visibility.Visible;
        }
    }
}