using System.Drawing.Imaging;
using System.Drawing;
using NAudio.Wave;
using NAudio.WaveFormRenderer;

namespace Обрезка_аудио
{
    class AudioRender
    {
        public AudioRender()
        {
            var myPeakProvider = new RmsPeakProvider(200);

            //var topSpacerColor = Color.FromArgb(255, 255, 255, 255);
            //var myRendererSettings = new SoundCloudBlockWaveFormSettings(Color.FromArgb(196, 197, 53, 0), topSpacerColor, Color.FromArgb(196, 79, 26, 0),
            //    topSpacerColor)
            //{
            //    Width = 2560,
            //    TopHeight = 100,
            //    BottomHeight = 100
            //};

            var myRendererSettings = new StandardWaveFormRendererSettings()
            {
                Width = 1000,
                TopHeight = 100,
                BottomHeight = 100,
                TopPeakPen = new Pen(Color.FromArgb(196, 197, 53, 0)),
                BottomPeakPen = new Pen(Color.FromArgb(196, 79, 26, 0)),
                BackgroundColor = Color.White
            };

            var renderer = new WaveFormRenderer();
            var audioFilePath = "C:\\Users\\naitm\\Desktop\\ГЗ\\Обрезка аудио\\Image\\Tokijjskijj_gul_-_Opening_original_1_sezon_60672129.mp3";
            var waveStream = new AudioFileReader(audioFilePath);
            var image = renderer.Render(waveStream, myPeakProvider, myRendererSettings);

            image.Save("C:\\Users\\naitm\\Desktop\\ГЗ\\Обрезка аудио\\Image\\waveform.png", ImageFormat.Png);
        }
    }
}
