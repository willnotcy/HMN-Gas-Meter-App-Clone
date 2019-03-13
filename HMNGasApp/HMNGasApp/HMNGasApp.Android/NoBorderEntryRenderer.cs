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
    public class NoBorderEntryRenderer : EntryRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}