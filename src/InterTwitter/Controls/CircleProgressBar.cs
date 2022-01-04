using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace InterTwitter.Controls
{
    public class CircleProgressBar : ContentView
    {
        private SKCanvasView _canvasView;

        public CircleProgressBar()
        {
            _canvasView = new SKCanvasView();
            _canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = _canvasView;
        }

        #region -- Public properties --

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(
            propertyName: nameof(Value),
            returnType: typeof(int),
            declaringType: typeof(CircleProgressBar),
            defaultBindingMode: BindingMode.TwoWay);

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
            propertyName: nameof(Minimum),
            returnType: typeof(int),
            declaringType: typeof(CircleProgressBar),
            defaultBindingMode: BindingMode.TwoWay);

        public int Minimum
        {
            get => (int)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
            propertyName: nameof(Maximum),
            returnType: typeof(int),
            declaringType: typeof(CircleProgressBar),
            defaultValue: 100,
            defaultBindingMode: BindingMode.TwoWay);

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(CircleProgressBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            propertyName: nameof(TextColor),
            returnType: typeof(Color),
            declaringType: typeof(CircleProgressBar),
            defaultValue: Color.Black,
            defaultBindingMode: BindingMode.TwoWay);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty FontScaleProperty = BindableProperty.Create(
            propertyName: nameof(FontScale),
            returnType: typeof(float),
            declaringType: typeof(CircleProgressBar),
            defaultValue: 1f,
            defaultBindingMode: BindingMode.TwoWay);

        public float FontScale
        {
            get => (float)GetValue(FontScaleProperty);
            set => SetValue(FontScaleProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            propertyName: nameof(FontFamily),
            returnType: typeof(string),
            declaringType: typeof(CircleProgressBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
            propertyName: nameof(FontAttributes),
            returnType: typeof(SKTypefaceStyle),
            declaringType: typeof(CircleProgressBar),
            defaultValue: SKTypefaceStyle.Normal,
            defaultBindingMode: BindingMode.TwoWay);

        public SKTypefaceStyle FontAttributes
        {
            get => (SKTypefaceStyle)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }

        public static readonly BindableProperty LineWidthProperty = BindableProperty.Create(
            propertyName: nameof(LineWidth),
            returnType: typeof(int),
            declaringType: typeof(CircleProgressBar),
            defaultValue: 1,
            defaultBindingMode: BindingMode.TwoWay);

        public int LineWidth
        {
            get => (int)GetValue(LineWidthProperty);
            set => SetValue(LineWidthProperty, value);
        }

        public static readonly BindableProperty ProgressLineColorProperty = BindableProperty.Create(
            propertyName: nameof(ProgressLineColor),
            returnType: typeof(Color),
            declaringType: typeof(CircleProgressBar),
            defaultValue: Color.Blue,
            defaultBindingMode: BindingMode.TwoWay);

        public Color ProgressLineColor
        {
            get => (Color)GetValue(ProgressLineColorProperty);
            set => SetValue(ProgressLineColorProperty, value);
        }

        public static readonly BindableProperty BackgroundLineColorProperty = BindableProperty.Create(
            propertyName: nameof(BackgroundLineColor),
            returnType: typeof(Color),
            declaringType: typeof(CircleProgressBar),
            defaultValue: Color.Silver,
            defaultBindingMode: BindingMode.TwoWay);

        public Color BackgroundLineColor
        {
            get => (Color)GetValue(BackgroundLineColorProperty);
            set => SetValue(BackgroundLineColorProperty, value);
        }

        public static readonly BindableProperty BackgroundProgressColorProperty = BindableProperty.Create(
            propertyName: nameof(BackgroundProgressColor),
            returnType: typeof(Color),
            declaringType: typeof(CircleProgressBar),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.TwoWay);

        public Color BackgroundProgressColor
        {
            get => (Color)GetValue(BackgroundProgressColorProperty);
            set => SetValue(BackgroundProgressColorProperty, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Value):
                case nameof(Minimum):
                case nameof(Maximum):
                case nameof(Text):
                case nameof(TextColor):
                case nameof(FontScale):
                case nameof(FontFamily):
                case nameof(FontAttributes):
                case nameof(LineWidth):
                case nameof(ProgressLineColor):
                case nameof(BackgroundLineColor):
                case nameof(BackgroundProgressColor):
                    _canvasView.InvalidateSurface();
                    break;
            }
        }

        #endregion

        #region -- Private methods --

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);

            var minRadius = Math.Min(info.Width, info.Height);

            var lineWidthPercent = LineWidth * minRadius / 200;
            var radiusWithLine = center.Y - (lineWidthPercent / 2);
            var innerRadius = radiusWithLine - (lineWidthPercent / 2);
            var maxWidthText = (float)Math.Sqrt(2 * innerRadius * innerRadius);

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = BackgroundLineColor.ToSKColor(),
                StrokeWidth = lineWidthPercent,
            };

            canvas.DrawCircle(center.X, center.Y, radiusWithLine, paint);

            paint.Style = SKPaintStyle.Fill;
            paint.Color = BackgroundProgressColor.ToSKColor();
            canvas.DrawCircle(center.X, center.Y, innerRadius, paint);

            if (Value < Minimum)
            {
                Value = Minimum;
            }

            if (Value > Maximum)
            {
                Value = Maximum;
            }

            if (Value == Maximum)
            {
                SKPaint paint2 = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = ProgressLineColor.ToSKColor(),
                    StrokeWidth = lineWidthPercent,
                };

                canvas.DrawCircle(center.X, center.Y, radiusWithLine, paint2);
            }
            else
            {
                using (SKPath path = new SKPath())
                {
                    float range = Maximum - Minimum;
                    float progress = Value - Minimum;
                    float limit = (360 * progress / range) - 90;

                    limit = limit == -90f ? limit : limit + 0.1f;

                    for (float angle = -90; angle < limit; angle += 0.05f)
                    {
                        double radians = Math.PI * angle / 180;
                        float x = center.X + (radiusWithLine * (float)Math.Cos(radians));
                        float y = center.Y + (radiusWithLine * (float)Math.Sin(radians));

                        SKPoint point = new SKPoint(x, y);

                        if (angle == -90)
                        {
                            path.MoveTo(point);
                        }
                        else
                        {
                            path.LineTo(point);
                        }
                    }

                    paint.Style = SKPaintStyle.Stroke;
                    paint.Color = ProgressLineColor.ToSKColor();
                    paint.StrokeWidth = lineWidthPercent;
                    paint.StrokeCap = SKStrokeCap.Round;

                    canvas.DrawPath(path, paint);
                }
            }

            if (!string.IsNullOrEmpty(Text))
            {
                var textWidth = maxWidthText * FontScale;
                if (textWidth > maxWidthText)
                {
                    textWidth = maxWidthText;
                }

                SKFont textFont = new SKFont
                {
                    Typeface = SKTypeface.FromFamilyName(FontFamily, FontAttributes),
                    Size = textWidth,
                };

                SKPaint textPaint = new SKPaint
                {
                    Color = TextColor.ToSKColor(),
                };
                textPaint.TextSize = textWidth;

                var textBounds = default(SKRect);
                textPaint.MeasureText(Text, ref textBounds);

                canvas.DrawText(Text, center.X - textBounds.MidX, center.Y - textBounds.MidY, textFont, textPaint);
            }
        }

        #endregion
    }
}