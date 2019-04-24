﻿using HMNGasApp.Helpers;
using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.View;
using HMNGasApp.ViewModel;
using HMNGasApp.WebServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HMNGasApp
{
    public partial class App : Application
    {
        private readonly Lazy<IServiceProvider> _lazyProvider = new Lazy<IServiceProvider>(() => ConfigureServices());

        private static readonly Uri _backendUrl = new Uri("https://localhost:44336/");

        public IServiceProvider Container => _lazyProvider.Value;

        public App()
        {
            InitializeComponent();
            DependencyResolver.ResolveUsing(type => Container.GetService(type));
            DemoStuff();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            var context = DependencyService.Resolve<IConfig>().Context;
            if (context.securityKey != null && context.securityKey != "")
            {
                var service = DependencyService.Get<ILoginSoapService>();
                Task.Run(async () => await service.Logout());
                MainPage = new NavigationPage(new LoginPage());
            }
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
            services.AddScoped<ReadingConfirmationPageViewModel>();
			services.AddScoped<UsagePageViewModel>();
			services.AddSingleton<IConfig>(config);
            services.AddScoped<ILoginSoapService, LoginSoapService>();
            services.AddScoped<ICustomerSoapService, CustomerSoapService>();
            services.AddScoped<IMeterReadingSoapService, MeterReadingSoapService>();
            services.AddSingleton<IXellentAPI, XellentAPI>();
            services.AddScoped<IConnectService, ConnectService>();
            services.AddScoped<IJSONRepository, JSONRepository>();


            return services.BuildServiceProvider();
        }

        private async Task DemoStuff()
        {
            var repo = DependencyService.Resolve<IJSONRepository>();
            var res = await repo.Read();
            System.Diagnostics.Debug.WriteLine(res);
        }
    }
}