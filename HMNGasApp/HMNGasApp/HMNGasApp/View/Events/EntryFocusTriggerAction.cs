using HMNGasApp.View.Components;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HMNGasApp.View.Events
{
    public class EntryFocusTriggerAction : TriggerAction<NoBorderEntry>
    {
        public bool Focused { get; set; }

        protected override async void Invoke(NoBorderEntry sender)
        {
            await Task.Delay(200);

            if (Focused)
            {
                sender.Focus();
            }
        }
    }
}