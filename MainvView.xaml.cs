using DynamicData;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // ⭐️ ObservableCollection을 위해 필수
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
// ModelMain 클래스를 사용하기 위해 네임스페이스를 참조합니다.
using WpfSimpleCal.modelMain;
using WpfSimpleCal.RelayCommands;

namespace WpfSimpleCal
{
    // ViewModel은 INotifyPropertyChanged를 구현해야 합니다.
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}