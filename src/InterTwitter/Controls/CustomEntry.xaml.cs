using InterTwitter.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.Controls
{
    public partial class CustomEntry : Grid
    {
        public CustomEntry()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
            propertyName: nameof(MaxLength),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string MaxLength
        {
            get => (string)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            propertyName: nameof(TextColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEntry),
            defaultValue: Color.Silver,
            defaultBindingMode: BindingMode.TwoWay);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            propertyName: nameof(FontFamily),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            propertyName: nameof(Placeholder),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
            propertyName: nameof(PlaceholderColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEntry),
            defaultValue: Color.Silver,
            defaultBindingMode: BindingMode.TwoWay);

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public static readonly BindableProperty IsEntryFocusedProperty = BindableProperty.Create(
            propertyName: nameof(IsEntryFocused),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsEntryFocused
        {
            get => (bool)GetValue(IsEntryFocusedProperty);
            set => SetValue(IsEntryFocusedProperty, value);
        }

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            propertyName: nameof(IsPassword),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public static readonly BindableProperty IsPasswordHiddenProperty = BindableProperty.Create(
            propertyName: nameof(IsPasswordHidden),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsPasswordHidden
        {
            get => (bool)GetValue(IsPasswordHiddenProperty);
            set => SetValue(IsPasswordHiddenProperty, value);
        }

        public static readonly BindableProperty IsFocusedVisibleProperty = BindableProperty.Create(
            propertyName: nameof(IsFocusedVisible),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsFocusedVisible
        {
            get => (bool)GetValue(IsFocusedVisibleProperty);
            set => SetValue(IsFocusedVisibleProperty, value);
        }

        public static readonly BindableProperty IsFocusedButtonProperty = BindableProperty.Create(
            propertyName: nameof(IsFocusedButton),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsFocusedButton
        {
            get => (bool)GetValue(IsFocusedButtonProperty);
            set => SetValue(IsFocusedButtonProperty, value);
        }

        public static readonly BindableProperty IsButtonEyeVisibleProperty = BindableProperty.Create(
            propertyName: nameof(IsButtonEyeVisible),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsButtonEyeVisible
        {
            get => (bool)GetValue(IsButtonEyeVisibleProperty);
            set => SetValue(IsButtonEyeVisibleProperty, value);
        }

        public static readonly BindableProperty IsButtonClearVisibleProperty = BindableProperty.Create(
            propertyName: nameof(IsButtonClearVisible),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsButtonClearVisible
        {
            get => (bool)GetValue(IsButtonClearVisibleProperty);
            set => SetValue(IsButtonClearVisibleProperty, value);
        }

        public static readonly BindableProperty ClearImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(ClearImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string ClearImageSource
        {
            get => (string)GetValue(ClearImageSourceProperty);
            set => SetValue(ClearImageSourceProperty, value);
        }

        public static readonly BindableProperty EyeOnImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(EyeOnImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string EyeOnImageSource
        {
            get => (string)GetValue(EyeOnImageSourceProperty);
            set => SetValue(EyeOnImageSourceProperty, value);
        }

        public static readonly BindableProperty EyeOffImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(EyeOffImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string EyeOffImageSource
        {
            get => (string)GetValue(EyeOffImageSourceProperty);
            set => SetValue(EyeOffImageSourceProperty, value);
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(ImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly BindableProperty IsSwapButtonsProperty = BindableProperty.Create(
           propertyName: nameof(IsSwapButtons),
           returnType: typeof(bool),
           declaringType: typeof(CustomEntry),
           defaultBindingMode: BindingMode.TwoWay);

        public bool IsSwapButtons
        {
            get => (bool)GetValue(IsSwapButtonsProperty);
            set => SetValue(IsSwapButtonsProperty, value);
        }

        public static readonly BindableProperty Eye_grid_columnProperty = BindableProperty.Create(
           propertyName: nameof(Eye_grid_column),
           returnType: typeof(int),
           declaringType: typeof(CustomEntry),
           defaultValue: 1,
           defaultBindingMode: BindingMode.TwoWay);

        public int Eye_grid_column
        {
            get => (int)GetValue(Eye_grid_columnProperty);
            set => SetValue(Eye_grid_columnProperty, value);
        }

        public static readonly BindableProperty Cross_grid_columnProperty = BindableProperty.Create(
           propertyName: nameof(Cross_grid_column),
           returnType: typeof(int),
           declaringType: typeof(CustomEntry),
           defaultValue: 2,
           defaultBindingMode: BindingMode.TwoWay);

        public int Cross_grid_column
        {
            get => (int)GetValue(Cross_grid_columnProperty);
            set => SetValue(Cross_grid_columnProperty, value);
        }

        public static readonly BindableProperty IsButtonClearEnableProperty = BindableProperty.Create(
          propertyName: nameof(IsButtonClearEnable),
          returnType: typeof(bool),
          declaringType: typeof(CustomEntry),
          defaultValue: true,
          defaultBindingMode: BindingMode.TwoWay);

        public bool IsButtonClearEnable
        {
            get => (bool)GetValue(IsButtonClearEnableProperty);
            set => SetValue(IsButtonClearEnableProperty, value);
        }

        private ICommand _buttonEyeCommand;
        public ICommand ButtonEyeCommand => _buttonEyeCommand ??= SingleExecutionCommand.FromFunc(OnButtonEyeCommandAsync);

        private ICommand _buttonClearCommand;
        public ICommand ButtonClearCommand => _buttonClearCommand ??= SingleExecutionCommand.FromFunc(OnButtonClearCommandAsync);

        private ICommand _focusedCommand;
        public ICommand FocusedCommand => _focusedCommand ??= SingleExecutionCommand.FromFunc(OnFocusedCommandAsync);

        private ICommand _unfocusedCommand;
        public ICommand UnFocusedCommand => _unfocusedCommand ??= SingleExecutionCommand.FromFunc(OnUnfocusedCommandAsync);

        #endregion

        #region -- Overrides --

        protected override async void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(IsPassword):
                    IsPasswordHidden = IsPassword;
                    break;
                case nameof(IsSwapButtons):
                    {
                        if (IsSwapButtons)
                        {
                            Eye_grid_column = 2;
                            Cross_grid_column = 1;
                        }
                        else
                        {
                            Eye_grid_column = 1;
                            Cross_grid_column = 2;
                        }
                    }

                    break;
                case nameof(Text):
                case nameof(ClearImageSource):
                case nameof(EyeOnImageSource):
                case nameof(EyeOffImageSource):

                    if (IsPassword)
                    {
                        if (IsPasswordHidden)
                        {
                            ImageSource = EyeOnImageSource;
                        }
                        else
                        {
                            ImageSource = EyeOffImageSource;
                        }

                        if (string.IsNullOrEmpty(Text))
                        {
                            IsButtonEyeVisible = false;
                        }
                        else
                        {
                            IsButtonEyeVisible = true;
                        }
                    }

                    if (string.IsNullOrEmpty(Text))
                    {
                        IsButtonClearVisible = false;
                    }
                    else
                    {
                        IsButtonClearVisible = true;
                    }

                    break;
            }

            if (propertyName == nameof(IsEntryFocused))
            {
                if (IsEntryFocused)
                {
                    await Task.Delay(200);
                    CustomEntryLocal.Focus();
                    IsEntryFocused = false;
                }
            }
        }

        #endregion

        #region -- Private methods --

        private Task OnButtonEyeCommandAsync()
        {
            if (IsPassword)
            {
                if (ImageSource == EyeOnImageSource)
                {
                    IsPasswordHidden = false;
                    ImageSource = EyeOffImageSource;
                }
                else
                {
                    IsPasswordHidden = true;
                    ImageSource = EyeOnImageSource;
                }
            }

            return Task.CompletedTask;
        }

        private async Task OnFocusedCommandAsync()
        {
            if (!IsPassword)
            {
                IsButtonEyeVisible = true;
            }

            IsFocusedButton = true;
            CustomEntryLocal.Placeholder = string.Empty;

            await Task.Delay(300);
            IsFocusedVisible = true;
        }

        private Task OnUnfocusedCommandAsync()
        {
            if (!IsPassword)
            {
                IsButtonEyeVisible = false;
            }

            if (string.IsNullOrEmpty(Text))
            {
                IsFocusedVisible = false;
            }

            IsFocusedButton = false;
            CustomEntryLocal.Placeholder = Placeholder;

            return Task.CompletedTask;
        }

        private Task OnButtonClearCommandAsync()
        {
            Text = string.Empty;
            IsFocusedVisible = false;

            return Task.CompletedTask;
        }

        #endregion

    }
}
