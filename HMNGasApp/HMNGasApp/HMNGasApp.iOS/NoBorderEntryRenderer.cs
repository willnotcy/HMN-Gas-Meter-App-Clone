using HMNGasApp.iOS;
using HMNGasApp.View.Components;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NoBorderEntry), typeof(NoBorderEntryRenderer))]
namespace HMNGasApp.iOS
{
    public class NoBorderEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
            
        }
    }
}