using System;
using System.Threading.Tasks;

namespace AppVendas.Interface
{
    public interface IAlertService
    {
        Task ShowAsync(string title, string message, string buttonOk);
    }
}
