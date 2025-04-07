using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для ThreeSlider.xaml
    /// </summary>
    public partial class ThreeSlider : UserControl
    {
        public event EventHandler LeftValueChanged;
        public event EventHandler MiddleValueChanged;
        public event EventHandler RightValueChanged;

        public ThreeSlider()
        {
            InitializeComponent();
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(ThreeSlider), new UIPropertyMetadata(0d)); 

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(ThreeSlider), new UIPropertyMetadata(0d));

        public double LeftValue
        {
            get { return (double)GetValue(LeftValueProperty); }
            set { SetValue(LeftValueProperty, value); }
        }
        public static readonly DependencyProperty LeftValueProperty = DependencyProperty.Register("LeftValue", typeof(double), typeof(ThreeSlider), new UIPropertyMetadata(0d));

        public double MiddleValue
        {
            get { return (double)GetValue(MiddleValueProperty); }
            set { SetValue(MiddleValueProperty, value); }
        }
        public static readonly DependencyProperty MiddleValueProperty = DependencyProperty.Register("MiddleValue", typeof(double), typeof(ThreeSlider), new UIPropertyMetadata(0d));

        public double RightValue
        {
            get { return (double)GetValue(RightValueProperty); }
            set { SetValue(RightValueProperty, value); }
        }
        public static readonly DependencyProperty RightValueProperty = DependencyProperty.Register("RightValue", typeof(double), typeof(ThreeSlider), new UIPropertyMetadata(0d));

        private void LeftSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LeftValueChanged(this, e);
            rightSlider.Value = Math.Max(rightSlider.Value, leftSlider.Value);
            middleSlider.Value = Math.Max(middleSlider.Value, leftSlider.Value);
        }

        public void MiddleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MiddleValueChanged(this, e);
            leftSlider.Value = Math.Min(middleSlider.Value, leftSlider.Value);
            rightSlider.Value = Math.Max(middleSlider.Value, rightSlider.Value);
        }

        private void RightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RightValueChanged(this, e);
            leftSlider.Value = Math.Min(rightSlider.Value, leftSlider.Value);
            middleSlider.Value = Math.Min(middleSlider.Value, rightSlider.Value);
        }

    }
}
