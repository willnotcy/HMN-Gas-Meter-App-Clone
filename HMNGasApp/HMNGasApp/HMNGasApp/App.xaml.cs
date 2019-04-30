using HMNGasApp.Helpers;
using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.View;
using HMNGasApp.ViewModel;
using HMNGasApp.WebServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Net.Http;
using System.Diagnostics.CodeAnalysis;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HMNGasApp
{
    [ExcludeFromCodeCoverage]
    public partial class App : Application
    {
        private readonly Lazy<IServiceProvider> _lazyProvider = new Lazy<IServiceProvider>(() => ConfigureServices());

        private static readonly Uri _backendUrl = new Uri("https://gasnet.dk/GasApp/File/");

        public IServiceProvider Container => _lazyProvider.Value;

        public App()
        {
            InitializeComponent();
            DependencyResolver.ResolveUsing(type => Container.GetService(type));

            // Retrieve resource dictionary values from HMN server and update where needed. Runs in a new thread and updates when ready.
            Task.Run(() => LoadJSON()).Wait();

            MainPage = new NavigationPage(new LoginPage());
        }

        private async Task LoadJSON()
        {
            var json = DependencyService.Resolve<IJSONRepository>();

			var dic = await json.Read();;
			foreach (KeyValuePair<string, string> entry in dic)
			{
				if (!entry.Value.Equals(""))
				{
					Application.Current.Resources[entry.Key] = entry.Value;
				}
			}
        }

        protected override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var context = new UserContext { Caller = "", Company = "", functionName = "", Logg = 0, MaxRows = 1, StartRow = 0, securityKey = "" };

            var config = new Config
            {
                ApiKey = Secrets.ApiKey,
                Context = context
            };

            services.AddScoped<LoginViewModel>();
            services.AddScoped<InfoViewModel>();
            services.AddScoped<MainPageViewModel>();
            services.AddScoped<ManualPageViewModel>();
            services.AddScoped<ScanViewModel>();
            services.AddSingleton<IUserContext>(context);
            services.AddScoped<ReadingConfirmationPageViewModel>();
			services.AddScoped<UsagePageViewModel>();
			services.AddSingleton<IConfig>(config);
            services.AddScoped<ILoginSoapService, LoginSoapService>();
            services.AddScoped<ICustomerSoapService, CustomerSoapService>();
            services.AddScoped<IMeterReadingSoapService, MeterReadingSoapService>();
            services.AddSingleton<IXellentAPI, XellentAPI>();
            services.AddScoped<IConnectService, ConnectService>();
            services.AddScoped<IJSONRepository, JSONRepository>();
            services.AddSingleton(_ => new HttpClient() { BaseAddress = _backendUrl });

            return services.BuildServiceProvider();
        }
    }
}