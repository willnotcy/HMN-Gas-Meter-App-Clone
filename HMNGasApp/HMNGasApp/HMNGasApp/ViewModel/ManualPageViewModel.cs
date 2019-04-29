﻿using System;
using System.Windows.Input;
using Xamarin.Forms;
using HMNGasApp.View;
using System.Threading.Tasks;

namespace HMNGasApp.ViewModel
{
    public class ManualPageViewModel : BaseViewModel
    {
        public ICommand ManualCommand { get; set; }
        public ICommand ReturnNavCommand { get; set; }

        private string _titleText;
        public string TitleText
        {
            get => _titleText;
            set => SetProperty(ref _titleText, value);
        }

        private string _placeholder;
        public string Placeholder
        {
            get => _placeholder;
            set => SetProperty(ref _placeholder, value);
        }

        private string _intructionsText;
        public string IntructionsText
        {
            get => _intructionsText;
            set => SetProperty(ref _intructionsText, value);
        }

        private string _usageInput;
        public string UsageInput
        {
            get => _usageInput;
            set => SetProperty(ref _usageInput, value);
        }

        private string _phoneText;
        public string PhoneText
        {
            get => _phoneText;
            set => SetProperty(ref _phoneText, value);
        }

        private string _exampleText;
        public string ExampleText
        {
            get => _exampleText;
            set => SetProperty(ref _exampleText, value);
        }

        private string _examplePicture;
        public string ExamplePicture
        {
            get => _examplePicture;
            set => SetProperty(ref _examplePicture, value);
        }

        public ManualPageViewModel()
        {
            Init();
            ManualCommand = new Command(async () => await ExecuteManualCommand());
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
        }

        private async Task ExecuteReturnNavCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PopAsync();

            IsBusy = false;
        }

        private async Task ExecuteManualCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            if (UsageInput == null || UsageInput.Equals(""))
            {
                await App.Current.MainPage.DisplayAlert("Fejl", "Input feltet må ikke være tomt!", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ReadingConfirmationPage(UsageInput));
            }
            IsBusy = false;
        }
        public void Init()
        {
            _titleText = "Manuel Indtastning";
            _placeholder = "Måleraflæsning...";
            _intructionsText = "Indtast din måleraflæsning:";
            _phoneText = "Eller indmeld via tlf.: +45 62 25 90 00";
            _exampleText = "Bemærk: det er kun tallene inden for det orange felt der skal indsendes.";
            _examplePicture = "meter_example.jpg";
        }
        public void Reset()
        {
            UsageInput = null;
        }
    }
}
