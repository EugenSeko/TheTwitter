using Xamarin.Forms;

namespace InterTwitter.Controls
{
    public class CustomEditor : Editor
    {
        public CustomEditor()
        {
            TextChanged += OnTextChanged;
        }

        #region -- Public properties --

        public static readonly BindableProperty IsExpandableProperty = BindableProperty.Create(
            propertyName: nameof(IsExpandable),
            returnType: typeof(bool),
            declaringType: typeof(CustomEditor),
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsExpandable
        {
            get => (bool)GetValue(IsExpandableProperty);
            set => SetValue(IsExpandableProperty, value);
        }

        public static readonly BindableProperty CorrectLengthProperty = BindableProperty.Create(
            propertyName: nameof(CorrectLength),
            returnType: typeof(int),
            declaringType: typeof(CustomEditor),
            defaultValue: 250,
            defaultBindingMode: BindingMode.TwoWay);

        public int CorrectLength
        {
            get => (int)GetValue(CorrectLengthProperty);
            set => SetValue(CorrectLengthProperty, value);
        }

        public static readonly BindableProperty OverflowLengthColorProperty = BindableProperty.Create(
            propertyName: nameof(OverflowLengthColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEditor),
            defaultValue: Color.Red,
            defaultBindingMode: BindingMode.TwoWay);

        public Color OverflowLengthColor
        {
            get => (Color)GetValue(OverflowLengthColorProperty);
            set => SetValue(OverflowLengthColorProperty, value);
        }

        #endregion

        #region -- Private methods --

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsExpandable)
            {
                InvalidateMeasure();
            }
        }

        #endregion
    }
}