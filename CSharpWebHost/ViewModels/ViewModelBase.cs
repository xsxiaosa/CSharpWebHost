using ReactiveUI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSharpWebHost.ViewModels
{
    public class ViewModelBase : ReactiveObject, INotifyPropertyChanged, IDisposable
    {
        private bool _isBusy;
        private string _errorMessage;

        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Dispose()
        {
            // 清理资源
        }

        protected void SetError(Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
