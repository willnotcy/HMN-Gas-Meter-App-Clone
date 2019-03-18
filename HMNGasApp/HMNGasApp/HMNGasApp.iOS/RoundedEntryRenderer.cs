using HMNGasApp.iOS;
using HMNGasApp.View.Components;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace HMNGasApp.iOS
{
    /// <summary>
    /// Class for creating rounded borders in Entries, iOS
    /// </summary>
    public class RoundedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.RoundedRect;
            }
        }
    }
}