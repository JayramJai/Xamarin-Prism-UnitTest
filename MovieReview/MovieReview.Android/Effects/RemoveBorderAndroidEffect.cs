using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using MovieReview.Droid.Effects;
using MovieReview.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("MovieReview")]
[assembly: ExportEffect(typeof(RemoveBorderAndroidEffect), nameof(RemoveBorderEffect))]
namespace MovieReview.Droid.Effects
{
    public class RemoveBorderAndroidEffect : PlatformEffect
    {
        Drawable? originalBackground;

        protected override void OnAttached()
        {
            originalBackground = Control.Background;

            var shape = new ShapeDrawable(new RectShape());
            if (shape.Paint != null)
            {
                shape.Paint.Color = global::Android.Graphics.Color.Transparent;
                shape.Paint.StrokeWidth = 0;
                shape.Paint.SetStyle(Paint.Style.Stroke);
            }
            Control.Background = shape;
        }

        protected override void OnDetached() => Control.Background = originalBackground;
    }
}