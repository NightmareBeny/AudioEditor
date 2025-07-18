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
    /// Логика взаимодействия для NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public event EventHandler UpValueChanged;
        public event EventHandler DownValueChanged;

        public NumericUpDown()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(NumericUpDown), new UIPropertyMetadata("00:00:00"));

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            UpValueChanged(this, e);
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            DownValueChanged(this, e);
        }
    }
}
