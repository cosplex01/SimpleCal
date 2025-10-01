using System.Drawing;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

namespace WpfSimpleCal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainIcon.Source = new BitmapImage(new Uri("/radiyuSD.png", UriKind.RelativeOrAbsolute));
            Searchbtn.Source = new BitmapImage(new Uri("/SearchButton.png", UriKind.RelativeOrAbsolute));
            BtnRadiyuSearch = new Button
            {
                Content = "Search Radiyu",
                Width = 330,
                Height = 150,
                Margin = new Thickness(10)
            };
        }

        private void BtnRadiyu1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}