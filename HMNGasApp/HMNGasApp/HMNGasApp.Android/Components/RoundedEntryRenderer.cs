using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HMNGasApp.Droid;
using HMNGasApp.View.Components;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace HMNGasApp.Droid
{
#pragma warning disable CS0618 // Type or member is obsolete
    /// <summary>
    /// Class for creating rounded borders in Entries, Android
    /// </summary>
    public class RoundedEntryRenderer : EntryRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = Forms.Context.GetDrawable(Resource.Drawable.RoundedEntryText);
                Control.Gravity = GravityFlags.CenterVertical;
                Control.SetPadding(80, 10, 10, 10);
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}