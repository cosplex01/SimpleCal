using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfSimpleCal.modelMain;
using WpfSimpleCal.RelayCommands;

// ⭐️ INotifyPropertyChanged 구현은 ViewModel이 담당합니다.
namespace WpfSimpleCal
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // 1. ListBox에 바인딩할 컬렉션 속성 (Model 타입을 담는 컬렉션)
        // 🚨 ViewModel은 자신의 클래스가 아닌, Model 클래스(modelMain)를 담아야 합니다.
        public ObservableCollection<modelMain.modelMain> DataItems { get; set; } = new ObservableCollection<modelMain.modelMain>();

        // 2. View에서 선택된 아이템을 받을 속성
        private modelMain.modelMain? _selectedItem;
        public modelMain.modelMain? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                // 선택된 아이템 로직 처리 (예: 상세 정보 표시)
            }
        }

        // 3. Command 속성 정의
        public ICommand AddItemCommand { get; private set; }
        public ICommand ReloadMemoryCommand { get; } // 메모리 확인 Command (이전 논의)
        public ICommand RemoveSelectedCommand { get; } // 선택 항목 제거 Command (이전 논의)

        // 4. ImageSource 속성 정의
        private ImageSource? _mainIcon;
        private ImageSource? _searchbtn;
        private ImageSource? _closebtn;

        public ImageSource? MainIcon
        {
            get => _mainIcon;
            set { _mainIcon = value; OnPropertyChanged(nameof(MainIcon)); }
        }
        public ImageSource? Searchbtn
        {
            get => _searchbtn;
            set { _searchbtn = value; OnPropertyChanged(nameof(Searchbtn)); }
        }
        public ImageSource? Closebtn
        {
            get => _closebtn;
            set { _closebtn = value; OnPropertyChanged(nameof(Closebtn)); }
        }

        // 5. 생성자 (모든 초기화 로직은 여기서 수행)
        public MainViewModel()
        {
            // 컬렉션 초기화는 선언 시 수행했습니다.

            // 이미지 로딩
            try
            {
                // 🚨 Uri 경로는 프로젝트 구조에 따라 수정해야 합니다.
                Uri MainIconImage = new Uri("pack://application:,,,/WpfSimpleCal;component/radiyuSD.png");
                Uri SearchbtImage = new Uri("pack://application:,,,/WpfSimpleCal;component/SearchButton.png");
                Uri ClosebtnImage = new Uri("pack://application:,,,/WpfSimpleCal;component/SCloseButton.png");

                MainIcon = new BitmapImage(MainIconImage);
                Searchbtn = new BitmapImage(SearchbtImage);
                Closebtn = new BitmapImage(ClosebtnImage);

                // 커맨드 초기화
                AddItemCommand = new RelayCommand(ExecuteAddItem);
                // 이전 논의의 Command 초기화:
                ReloadMemoryCommand = new RelayCommand(ExecuteReloadMemory);
                RemoveSelectedCommand = new RelayCommand(ExecuteRemoveSelected, CanExecuteRemoveSelected);

                // 초기 데이터 로드 (프로그램 시작 시 메모리 목록 로드)
                InitializeDefaultData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ViewModel 초기화 오류: {ex.Message}");
            }
        }

        // 6. 로직 메서드 정의
        private void InitializeDefaultData()
        {
            // 요구사항 1: 프로그램 시작 시 메모리 목록 로드
            // 임시 데이터:
            DataItems.Add(new modelMain.modelMain(name: "홍길동", id: 1, job: DateTime.UtcNow));
            DataItems.Add(new modelMain.modelMain(name: "임꺽정", id: 2, job: DateTime.UtcNow));

            // 실제 메모리 로드 로직은 LoadInitialProcesses() 함수에서 수행해야 합니다.
        }

        private void ExecuteAddItem(object parameter)
        {
            int newId = DataItems.Count + 1;
            var newModel = new modelMain.modelMain(name: $"프로그램 이름 {newId}", id: newId, job: DateTime.Now);
            DataItems.Add(newModel);
        }

        // ⭐️ 이전 논의에서 추가된 Command 실행 메서드 (예시)
        private void ExecuteReloadMemory(object parameter)
        {
            // 큐 정리 및 메모리 로드 로직
            Debug.WriteLine("메모리 재로딩 명령 실행");
        }
        private void ExecuteRemoveSelected(object parameter)
        {
            // 선택 항목 제거 로직
            Debug.WriteLine("선택 항목 제거 명령 실행");
        }
        private bool CanExecuteRemoveSelected(object parameter)
        {
            return SelectedItem != null; // 또는 SelectedProcesses.Any();
        }


        // 7. INotifyPropertyChanged 구현
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}