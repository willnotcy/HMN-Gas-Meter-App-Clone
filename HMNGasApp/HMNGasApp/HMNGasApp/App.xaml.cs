using HMNGasApp.Helpers;
using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.View;
using HMNGasApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HMNGasApp
{
    public partial class App : Application
    {
        private readonly Lazy<IServiceProvider> _lazyProvider = new Lazy<IServiceProvider>(() => ConfigureServices());

        public IServiceProvider Container => _lazyProvider.Value;

        public string securityKey = "";

        public App()
        {
            InitializeComponent();
            DependencyResolver.ResolveUsing(type => Container.GetService(type));
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var context = new UserContext();
            var config = new Config
            {
                ApiKey = Secrets.ApiKey
            };

            services.AddScoped<LoginViewModel>();
            services.AddScoped<InfoViewModel>();
            services.AddScoped<MainPageViewModel>();
            services.AddScoped<ManualPageViewModel>();
            services.AddSingleton<IUserContext>(context);
            services.AddSingleton<IConfig>(config);
            services.AddScoped<ILoginSoapService, LoginSoapService>();

            return services.BuildServiceProvider();
        }
    }
}
