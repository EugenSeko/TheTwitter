using InterTwitter.Models.TweetViewModel;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class MenuItemViewModel : BindableBase
    {
        public MenuItemViewModel()
        {
            TargetType = typeof(MenuItemViewModel);
        }

        #region -- Public Properties --

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private Type _targetType;
        public Type TargetType
        {
            get => _targetType;
            set => SetProperty(ref _targetType, value);
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        private ObservableCollection<BaseTweetViewModel> _contentCollection;
        public ObservableCollection<BaseTweetViewModel> ContentCollection
        {
            get => _contentCollection;
            set => SetProperty(ref _contentCollection, value);
        }

        private Xamarin.Forms.Color _textColor;
        public Xamarin.Forms.Color TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }

        private ICommand _tapCommand;
        public ICommand TapCommand
        {
            get => _tapCommand;
            set => SetProperty(ref _tapCommand, value);
        }

        #endregion

    }
}
