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
        //Get resources
        private readonly ResourceDictionary res = App.Current.Resources;

        public ICommand ManualPageNavCommand { get; set; }
        public ICommand InfoPageNavCommand { get; set; }
        public ICommand ScanPageNavCommand { get; }
        public ICommand LogOutCommand { get; set; }
        public ICommand SignOutCommand { get; set; }
		public ICommand UsagePageNavCommand { get; set; }

        public MainPageViewModel(ILoginSoapService service)
        {
            _service = service;

            ManualPageNavCommand = new Command(async () => await ExecuteManualPageNavCommand());
            InfoPageNavCommand = new Command(async () => await ExecuteInfoPageNavCommand());
			UsagePageNavCommand = new Command(async () => await ExecuteUsagePageNavCommand());
            ScanPageNavCommand = new Command(async () => await ExecuteScanPageNavCommand());
            SignOutCommand = new Command(async () => await ExecuteSignOutCommand());
        }
		
		public string _emergencyText;
        private string EmergencyText
        {
            get => _emergencyText;
            set => SetProperty(ref _emergencyText, value);
        }

        private async Task ExecuteScanPageNavCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PushAsync(new ScanPage());

            IsBusy = false;
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
                await App.Current.MainPage.DisplayAlert((String)res["Errors.Title.Fail"], (String)res["Errors.Message.SWW"], (String)res["Errors.Cancel.Okay"]);
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
	}
}
