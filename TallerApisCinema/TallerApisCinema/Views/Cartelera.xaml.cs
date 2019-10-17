using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TallerApisCinema.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TallerApisCinema.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Cartelera : ContentPage
	{

        public Cartelera ()
		{
			InitializeComponent ();
            CargarPeliculas();
		}

        private async void CargarPeliculas()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://misapis.azurewebsites.net");
            var request = await client.GetAsync("/api/cartelera");

            if (request.IsSuccessStatusCode)
            {
                var responseJson = await request.Content.ReadAsStringAsync();
                var listado = JsonConvert.DeserializeObject<List<Pelicula>>(responseJson);
                ListPeliculas.ItemsSource = listado;
            }
            else
            {
                await DisplayAlert("Lo sentimos!", "Ha ocurrido un error de comunicacion", "OK");
            }
        }

        private async void Pelicula_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var pelicula = (Pelicula)e.SelectedItem;
            await Navigation.PushAsync(new FuncionesPage(pelicula));
        }
    }
}