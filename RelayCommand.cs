using System;
using System.Windows.Input;

namespace WpfSimpleCal.RelayCommands // 혹은 해당 RelayCommand 클래스의 네임스페이스
{
    // ICommand 인터페이스를 구현하여 View에 Command 기능을 제공합니다.
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute; // Command 실행 메서드
        private Func<object, bool> _canExecute; // Command 활성화 여부 판단 메서드

        // 1. 인자가 하나인 생성자 (CanExecute가 없는 경우)
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        // 2. 인자가 두 개인 생성자 (사용자가 현재 찾고 있는 생성자)
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            // 실행 메서드는 null이 될 수 없습니다.
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            // 활성화 판단 메서드는 null이 허용됩니다.
            _canExecute = canExecute;
        }

        // Command 활성화/비활성화 상태 변경을 View에 알리는 이벤트
        public event EventHandler CanExecuteChanged
        {
            // WPF 표준 방식: CommandManager에 등록하여 UI 스레드에서 변경을 감지합니다.
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Command 실행 가능 여부를 판단합니다.
        public bool CanExecute(object parameter)
        {
            // _canExecute가 null이면 항상 true를 반환합니다. (즉시 실행 가능)
            return _canExecute == null || _canExecute(parameter);
        }

        // Command를 실행합니다.
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}