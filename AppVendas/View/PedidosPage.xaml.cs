using System;
using System.Collections.Generic;
using AppVendas.Model;
using AppVendas.ViewModel;
using Xamarin.Forms;

namespace AppVendas.View
{
    public partial class PedidosPage : ContentPage
    {
        public PedidosPage()
        {
            InitializeComponent();
            this.BindingContext = new MainPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((MainPageViewModel)BindingContext).ThisOnAppearing();
        }


    }
}
