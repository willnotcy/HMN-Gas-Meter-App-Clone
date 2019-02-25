using HMNGasApp.UWP;
using HMNGasApp.View.Components;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace HMNGasApp.UWP
{
    public class RoundedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Windows.UI.Xaml.ResourceDictionary dic = new Windows.UI.Xaml.ResourceDictionary();
                dic.Source = new Uri("ms-appx:///RoundedEntryRes.xaml");
                Control.Style = dic["RoundedEntryStyle"] as Windows.UI.Xaml.Style;
            }
        }
    }
}
