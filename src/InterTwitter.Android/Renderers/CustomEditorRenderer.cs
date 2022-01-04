using Android.Content;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using InterTwitter.Controls;
using InterTwitter.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace InterTwitter.Droid.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        private bool _isClear;

        public CustomEditorRenderer(Context context) 
            : base(context)
        {
        }

        #region -- Overrides --

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null && Element is CustomEditor editor)
            {
                switch (e.PropertyName)
                {
                    case nameof(editor.Text):
                        HighlightIfOverflowExists();
                        break;

                    case nameof(editor.IsExpandable):
                        Control.VerticalScrollBarEnabled = !editor.IsExpandable;
                        break;
                }
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null && Element is CustomEditor editor)
            {
                Control.SetPadding(0, 0, 0, 0);
                Control.SetBackgroundColor(e.NewElement.BackgroundColor.ToAndroid());
                Control.VerticalScrollBarEnabled = !editor.IsExpandable;

                HighlightIfOverflowExists();
            }
        }

        #endregion

        #region -- Private methods --

        private void HighlightIfOverflowExists()
        {
            var text = EditText.Text;

            if (!string.IsNullOrEmpty(text) && Control != null && Element is CustomEditor editor)
            {
                EditText.SetTextColor(editor.TextColor.ToAndroid());
                EditText.SetLineSpacing(18, 1);

                var length = text.Length;
                var correctLength = editor.CorrectLength;

                var pos = Control.SelectionEnd;

                if (length > correctLength)
                {
                    _isClear = true;

                    SpannableString spannable = new SpannableString(text);

                    spannable.SetSpan(new ForegroundColorSpan(
                        editor.OverflowLengthColor.ToAndroid()),
                        correctLength,
                        length,
                        SpanTypes.ExclusiveExclusive);

                    EditText.TextFormatted = spannable;

                    Control.SetSelection(pos);
                }
                else
                {
                    if (_isClear)
                    {
                        EditText.Text = text;
                        Control.SetSelection(pos);

                        _isClear = false;
                    }
                }
            }
        }

        #endregion
    }
}