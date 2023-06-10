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

namespace GrossErrorInputGeneratorWPF2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Settings settings;

        public MainWindow()
        {
            InitializeComponent();
            ChangeWindowPosition();
            settings = new Settings();
        }
        private void ChangeWindowPosition()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            settings.Borders = true;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            settings.Tolerance = true;
        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            settings.TestSchemes = true;
        }
        private void CheckBox_Checked_3(object sender, RoutedEventArgs e)
        {
            settings.Multiplier = this.TextBox1.Text == string.Empty ? 1 : int.Parse(this.TextBox1.Text);
        }
        private void CheckBox_Checked_4(object sender, RoutedEventArgs e)
        {
            settings.EmptySchemes = true;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.Borders = false;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            settings.Tolerance = false;
        }

        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            settings.TestSchemes = false;
        }

        private void CheckBox_Unchecked_3(object sender, RoutedEventArgs e)
        {
            settings.Multiplier = 1;
        }
        private void CheckBox_Unchecked_4(object sender, RoutedEventArgs e)
        {
            settings.EmptySchemes = false;
        }
        private void GenerateSchemes(object sender, RoutedEventArgs e)
        {
            
            IGenerator gen = new VariousSizeGenerator();
            if (settings.Multiplier > 1)
            {
                //settings.Multiplier = 3;
                //settings.Multiplier = 9;
                //settings.Multiplier = 12;
                gen.Generate(settings);
            }
            settings.Multiplier = this.TextBox1.Text == string.Empty ? 1 : int.Parse(this.TextBox1.Text);
            if (settings.Borders)
            {
                gen = new RandomBordersGenerator();
                gen.Generate(settings);
            }
            if (settings.Tolerance)
            {
                gen = new RandomToleranceGenerator();
                gen.Generate(settings);
            }
            if (settings.TestSchemes)
            {
                gen = new TestDataGenerator();
                gen.Generate(settings);
            }
            if (settings.Borders)
            {
                gen = new ZeroErrorsGeneraor();
                gen.Generate(settings);
            }
            MessageBox.Show("Схемы сгенерированы");
        }


    }
}
