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
        public ICommand? AddItemCommand { get; private set; }
        public ICommand? ClearListboxCommand { get; } // 선택 항목 제거 Command (이전 논의)
        public ICommand? ReloadMemoryCommand { get; } // 메모리 확인 Command (이전 논의)
        public ICommand? RemoveSelectedCommand { get; } // 선택 항목 제거 Command (이전 논의)

        // 4. ImageSource 속성 정의
        private ImageSource? _mainIcon;
        private ImageSource? _searchbtn;
        private ImageSource? _closebtn;
        private ImageSource? _clearlist;
        private ObservableCollection<modelMain.modelMain> processQueue = new ObservableCollection<modelMain.modelMain>();

        // Queue에 담긴 프로그램 리스트 (Listbox에 바인딩됨)
        public ObservableCollection<modelMain.modelMain> ProcessQueue { get => processQueue; set => processQueue = value; }
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
        public ImageSource? ClearList
        {
            get => _clearlist;
            set { _clearlist = value; OnPropertyChanged(nameof(ClearList)); }
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
                Uri ClearListImage = new Uri("pack://application:,,,/WpfSimpleCal;component/ClearList.png");

                MainIcon = new BitmapImage(MainIconImage);
                Searchbtn = new BitmapImage(SearchbtImage);
                Closebtn = new BitmapImage(ClosebtnImage);
                ClearList = new BitmapImage(ClearListImage);
                // 커맨드 초기화
                AddItemCommand = new RelayCommand(ExecuteAddItem);
                // 이전 논의의 Command 초기화:
                ReloadMemoryCommand = new RelayCommand(ExecuteReloadMemory);
                ClearListboxCommand = new RelayCommand(ExecuteClearListbox);
                RemoveSelectedCommand = new RelayCommand(ExecuteRemoveSelected, CanExecuteRemoveSelected);


                // 초기 데이터 로드 (프로그램 시작 시 메모리 목록 로드)
                //LoadInitialProcesses();
                //InitializeDefaultData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ViewModel 초기화 오류: {ex.Message}");
            }
        }

        private void LoadInitialProcesses()
        {
            // 1. 기존 데이터를 모두 지웁니다. (화면 새로 고침 역할)
            ProcessQueue.Clear();

            // 1. 메모리 상의 모든 실행 중인 프로세스를 가져옵니다.
            Process[] runningProcesses = Process.GetProcesses();

            // 2. 반복문을 사용하거나, LINQ를 사용하여 큐에 전부 담습니다.
            foreach (var process in runningProcesses.OrderBy(p => p.ProcessName).Take(50)) // 50개만 예시로
            {
                try
                {
                    // ID와 이름이 있는 프로세스만 필터링합니다.
                    if (!string.IsNullOrEmpty(process.ProcessName))
                    {
                        // 3. ProcessItem (Model) 객체로 변환하여 큐에 추가 (Enqueue)
                        modelMain.modelMain newItem = new modelMain.modelMain(
                            name : process.ProcessName,
                            id : process.Id,
                            job : process.StartTime // 큐에 담은 시간 기록
                        );
                        ProcessQueue.Add(newItem);
                    }
                }
                catch (Exception)
                {
                    // 권한 문제로 인해 일부 프로세스는 접근 불가능할 수 있음 (무시)
                    continue;
                }
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
            // 4. 실제로 프로세스를 로드하는 함수 호출
            LoadInitialProcesses();
            // 디버깅 메시지
            System.Diagnostics.Debug.WriteLine("검색/프로세스 로딩 커맨드 실행 완료");
            // 큐 정리 및 메모리 로드 로직
            Debug.WriteLine("메모리 재로딩 명령 실행");
        }
        private void ExecuteClearListbox(object parameter)
        {
            // 리스트박스 초기화 로직
            ProcessQueue.Clear();
            Debug.WriteLine("리스트박스 초기화 명령 실행");
            System.Diagnostics.Debug.WriteLine("화면 정리 커맨드 실행 완료: ListBox 초기화됨");
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