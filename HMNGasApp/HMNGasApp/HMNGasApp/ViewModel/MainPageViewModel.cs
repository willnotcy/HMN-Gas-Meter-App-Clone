using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HMNGasApp.Services;
using HMNGasApp.View;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ILoginSoapService _service;

        public ICommand ManualPageNavCommand { get; set; }
        public ICommand InfoPageNavCommand { get; set; }
        public ICommand SignOutCommand { get; set; }
		public ICommand UsagePageNavCommand { get; set; }

        public MainPageViewModel(ILoginSoapService service)
        {
            _service = service;

            ManualPageNavCommand = new Command(async () => await ExecuteManualPageNavCommand());
            InfoPageNavCommand = new Command(async () => await ExecuteInfoPageNavCommand());
			UsagePageNavCommand = new Command(async () => await ExecuteUsagePageNavCommand());
            SignOutCommand = new Command(async () => await ExecuteSignOutCommand());
        }
        public string _emergencyText;
        private string EmergencyText
        {
            get => _emergencyText;
            set => SetProperty(ref _emergencyText, value);
        }
        private async Task ExecuteSignOutCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var result = await _service.Logout();
            if (result)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Fejl", "Noget gik galt - prøv igen", "Okay");
            }

            IsBusy = false;
        }

        private async Task ExecuteInfoPageNavCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PushAsync(new InfoPage());

            IsBusy = false;
        }

        private async Task ExecuteManualPageNavCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PushAsync(new ManualPage());

            IsBusy = false;
        }

		private async Task ExecuteUsagePageNavCommand()
		{
			if (IsBusy)
			{
				return;
			}
			IsBusy = true;

			await Navigation.PushAsync(new UsagePage());

			IsBusy = false;
		}
        public void Init()
        {
            _emergencyText = "";
        }
	}
}
