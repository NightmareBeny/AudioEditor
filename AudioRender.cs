using System.Drawing.Imaging;
using System.Drawing;
using NAudio.Wave;
using NAudio.WaveFormRenderer;
using System.IO;

namespace Обрезка_аудио
{
    class AudioRender
    {
        public AudioRender() {}

        public string? PathToAudio { get; set; }

        public MemoryStream MakeImageFromAudio(string pathAudio)
        {
            //var topSpacerColor = Color.FromArgb(255, 255, 255, 255);
            //var myRendererSettings = new SoundCloudBlockWaveFormSettings(Color.FromArgb(196, 197, 53, 0), topSpacerColor, Color.FromArgb(196, 79, 26, 0),
            //    topSpacerColor)
            //{
            //    Width = 2560,
            //    TopHeight = 100,
            //    BottomHeight = 100
            //};
            var imgData = new MemoryStream();
            PathToAudio = pathAudio;
            var myRendererSettings = new StandardWaveFormRendererSettings()
            {
                Width = 1000,
                TopHeight = 100,
                BottomHeight = 100,
                TopPeakPen = new Pen(Color.FromArgb(196, 197, 53, 0)),
                BottomPeakPen = new Pen(Color.FromArgb(196, 79, 26, 0)),
                BackgroundColor = Color.White,
                PixelsPerPeak = 2
            };
            using (var waveStream = new AudioFileReader(pathAudio))
            {
                var image = new WaveFormRenderer().Render(waveStream, new RmsPeakProvider(200), myRendererSettings);
                image.Save(imgData, ImageFormat.Png);
            }
            return imgData;
        }
    }
}
