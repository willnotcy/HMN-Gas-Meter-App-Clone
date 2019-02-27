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

            services.AddScoped<LoginViewModel>();
            services.AddScoped<MainPageViewModel>();
            services.AddScoped<ManualPageViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
