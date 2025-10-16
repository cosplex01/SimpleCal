using System.Configuration;
using System.Data;
using System.Windows;
using WpfSimpleCal.RelayCommands; // ViewModel 네임스페이스
using WpfSimpleCal.modelMain; // Model 네임스페이스

namespace WpfSimpleCal
{
    public partial class App : Application
    {
        // ❌ viewModelDependency 필드는 제거하거나 사용하지 않아야 합니다.
        // private viewModel? viewModelDependency; 

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 1. ViewModel 생성. 
            // Model이나 다른 의존성이 있다면 생성자 인자로 전달합니다.
            // 인자가 없다면 괄호 없이 단순하게 생성합니다.
            MainViewModel mainViewModel = new MainViewModel();

            // 2. View 인스턴스 생성 (MainWindow.xaml)
            MainWindow mainWindow = new MainWindow();

            // 3. View와 ViewModel 연결 (DataContext 설정)
            // ⭐️ 이 한 줄이 모든 Binding을 작동시키는 핵심입니다!
            mainWindow.DataContext = mainViewModel;

            // 4. 창 표시
            mainWindow.Show();
        }
    }
}