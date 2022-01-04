using Prism.Mvvm;
using System.Windows.Input;

namespace InterTwitter.Models
{
    public class MiniCardModel : BindableBase
    {
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        private ICommand _actionCommand;
        public ICommand ActionCommand
        {
            get => _actionCommand;
            set => SetProperty(ref _actionCommand, value);
        }
    }
}
