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
<<<<<<< HEAD
            MainIcon.Source = new BitmapImage(new Uri("pack://application:,,,/radiyuSD.png"));            
=======
            MainIcon.Source = new BitmapImage(new Uri("/radiyuSD.png", UriKind.RelativeOrAbsolute));
>>>>>>> ecd113a4004b9251b4f52368e18bc3a631488d4b
        }
    }
}