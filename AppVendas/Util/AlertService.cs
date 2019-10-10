using System;
using System.Threading.Tasks;
using AppVendas.Interface;

namespace AppVendas.Util
{
    public class AlertService : IAlertService
    {
        public async Task ShowAsync(string title, string message, string buttonOk)
        {
            await App.Current.MainPage.DisplayAlert(title, message, buttonOk);
        }


    }
}
