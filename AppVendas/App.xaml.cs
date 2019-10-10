using System;
using AppVendas.Interface;
using AppVendas.Util;
using AppVendas.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppVendas
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //registra interface
            DependencyService.Register<IAlertService,AlertService>();

            MainPage = new NavigationPage(new PedidosPage());
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
    }
}
