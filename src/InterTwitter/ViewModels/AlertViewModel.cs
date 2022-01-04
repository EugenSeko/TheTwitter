using InterTwitter.Resources.Strings;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace InterTwitter.ViewModels
{
    public class AlertViewModel : BindableBase
    {
        public AlertViewModel(DialogParameters param, Action<IDialogParameters> requestClose)
        {
            SetupParameters(param);
            RequestClose = requestClose;
            CloseCommand = new DelegateCommand(() => RequestClose(null));
            AcceptCommand = new DelegateCommand(() => RequestClose(new DialogParameters() { { Constants.DialogParameterKeys.ACCEPT, true } }));
            DeclineCommand = new DelegateCommand(() => RequestClose(new DialogParameters() { { Constants.DialogParameterKeys.ACCEPT, false } }));
        }

        #region -- Public properties --

        public bool IsMessageVisible => !string.IsNullOrEmpty(Message);

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                SetProperty(ref _message, value);
                RaisePropertyChanged(nameof(IsMessageVisible));
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _okButtonText;
        public string OkButtonText
        {
            get => _okButtonText;
            set => SetProperty(ref _okButtonText, value);
        }

        private string _cancelButtonText;
        public string CancelButtonText
        {
            get => _cancelButtonText;
            set => SetProperty(ref _cancelButtonText, value);
        }

        public DelegateCommand CloseCommand { get; }
        public DelegateCommand AcceptCommand { get; }
        public DelegateCommand DeclineCommand { get; }

        #endregion

        public Action<IDialogParameters> RequestClose;

        private void SetupParameters(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(Constants.DialogParameterKeys.MESSAGE))
            {
                Message = parameters.GetValue<string>(Constants.DialogParameterKeys.MESSAGE);
                Title = Strings.Alert;
                OkButtonText = Strings.Ok;
            }

            if (parameters.ContainsKey(Constants.DialogParameterKeys.TITLE))
            {
                Title = parameters.GetValue<string>(Constants.DialogParameterKeys.TITLE);
            }

            if (parameters.ContainsKey(Constants.DialogParameterKeys.OK_BUTTON_TEXT))
            {
                OkButtonText = parameters.GetValue<string>(Constants.DialogParameterKeys.OK_BUTTON_TEXT);
            }

            if (parameters.ContainsKey(Constants.DialogParameterKeys.CANCEL_BUTTON_TEXT))
            {
                CancelButtonText = parameters.GetValue<string>(Constants.DialogParameterKeys.CANCEL_BUTTON_TEXT);
            }
        }
    }
}