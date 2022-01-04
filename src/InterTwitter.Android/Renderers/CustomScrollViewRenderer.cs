using Android.Content;
using InterTwitter.Controls;
using InterTwitter.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomScrollView), typeof(CustomScrollViewRenderer))]
namespace InterTwitter.Droid.Renderers
{
    public class CustomScrollViewRenderer : ScrollViewRenderer
    {
        public CustomScrollViewRenderer(Context context)
            : base(context)
        {

        }

        #region -- Overrides --
        
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement is CustomScrollView customScrollView)
            {
                OverScrollMode = customScrollView.Bounces
                    ? Android.Views.OverScrollMode.IfContentScrolls 
                    : Android.Views.OverScrollMode.Never;
            }
        }

        #endregion
    }
}