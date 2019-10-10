using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppVendas.Interface;
using AppVendas.Model;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppVendas.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {

        // endereco do firebase
        private readonly string ENDERECO_FIREBASE = "https://teste-sync-56418.firebaseio.com/";
        private IAlertService _alertService;
        private readonly FirebaseClient _firebaseClient;

        public MainPageViewModel()
        {
            this._alertService = DependencyService.Get<IAlertService>();

            _firebaseClient = new FirebaseClient(ENDERECO_FIREBASE);

            Pedidos = new ObservableCollection<Pedido>();

            NovoPedidoCmd = new Command(() => CriarNewPedido());

        }

        internal void ThisOnAppearing()
        {
            Pedidos.Clear();
            ListarPedido();
        }

        private ObservableCollection<Pedido> _pedidos;

        public ObservableCollection<Pedido> Pedidos
        {

            get { return _pedidos; }

            set { _pedidos = value; OnPropertyChanged(); }

        }

        private Pedido pedidoSelected;
        public Pedido PedidoSelected
        {
            get { return pedidoSelected; }
            set
            {
                try
                {
                    var pedido = (Pedido)value;

                    if (pedido == null)
                        return;

                    AceitarPedido(pedido);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }


        public ICommand NovoPedidoCmd { get; set; }

        private void ListarPedido()
        {
            _firebaseClient
                   .Child("pedidos")
                   .AsObservable<Pedido>()
                   .Subscribe(d =>
                   {
                       if (d.Object == null) return;

                       if (d.EventType == FirebaseEventType.InsertOrUpdate)
                       {
                         
                           if (d.Object.IdVendedor == 0)
                               AdicionarPedido(d.Key, d.Object);
                           else
                               RemoverPedido(d.Key);

                       }
                       else if (d.EventType == FirebaseEventType.Delete)
                       {
                           RemoverPedido(d.Key);
                       }
                   });
        }

        private void AdicionarPedido(string key, Pedido pedido)
        {
            Pedidos.Add(new Pedido()
            {

                KeyPedido = key,

                Cliente = pedido.Cliente,

                Produto = pedido.Produto,

                Valor = pedido.Valor

            });
        }

        private void RemoverPedido(string pedidoKey)
        {
            var pedido = Pedidos.FirstOrDefault(x => x.KeyPedido == pedidoKey);
            //remove o pedido
            Pedidos.Remove(pedido);
        }

        private void AceitarPedido(Pedido pedido)
        {

            pedido.IdVendedor = 1;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _alertService.ShowAsync("ATENÇÃO", "Não temos conexão com a internet.", "Ok");
                return;
            }

            // atualiza o firebase
            _firebaseClient
                    .Child("pedidos")
                    .Child(pedido.KeyPedido)
                    .PutAsync(pedido);
        }

        private void CriarNewPedido()
        {

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                 _alertService.ShowAsync("ATENÇÃO", "Não temos conexão com a internet.", "Ok");
                return;
            }

            Random random = new Random();

            Pedido pedido = new Pedido
            {
                KeyPedido = random.Next(5, 1000).ToString(),

                Cliente = "Cliente " + random.Next(5,1000),

                Produto = "Consórcio Yamaha YBR 125",

                Valor = random.Next(500, 1130),

                IdVendedor = 0
            };

            _firebaseClient
                .Child("pedidos").PostAsync(pedido);
        }

    }
}
