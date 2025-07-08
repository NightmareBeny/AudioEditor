using System.Drawing.Imaging;
using System.Drawing;
using NAudio.Wave;
using NAudio.WaveFormRenderer;

namespace Обрезка_аудио
{
    class AudioRender
    {
        public AudioRender() {}

        public string? PathToAudio { get; set; }

        public string MakeImageFromAudio(string pathAudio)
        {
            //var topSpacerColor = Color.FromArgb(255, 255, 255, 255);
            //var myRendererSettings = new SoundCloudBlockWaveFormSettings(Color.FromArgb(196, 197, 53, 0), topSpacerColor, Color.FromArgb(196, 79, 26, 0),
            //    topSpacerColor)
            //{
            //    Width = 2560,
            //    TopHeight = 100,
            //    BottomHeight = 100
            //};
            PathToAudio = pathAudio;
            var myRendererSettings = new StandardWaveFormRendererSettings()
            {
                Width = 1000,
                TopHeight = 100,
                BottomHeight = 100,
                TopPeakPen = new Pen(Color.FromArgb(196, 197, 53, 0)),
                BottomPeakPen = new Pen(Color.FromArgb(196, 79, 26, 0)),
                BackgroundColor = Color.White
            };
            var image = new WaveFormRenderer().Render(new AudioFileReader(pathAudio), new RmsPeakProvider(200), myRendererSettings);
            var index = Environment.ProcessPath.ToLower().IndexOf("обрезка аудио") + "обрезка аудио".Length + 1;
            var pathToImage = Environment.ProcessPath.Substring(0, index) + "Image\\waveform2.png";
            image.Save(pathToImage, ImageFormat.Png);
            return pathToImage;
        }
    }
}
