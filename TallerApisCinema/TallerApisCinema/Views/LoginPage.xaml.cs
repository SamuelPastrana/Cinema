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
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}

        private async void Ingresar_click(object sender, EventArgs e)
        {
            var usuario = Usuario.Text;
            var password = Password.Text;

            if (string.IsNullOrEmpty(usuario))
            {
                await DisplayAlert("Validacion", "Ingrese el usuario", "Aceptar");
                Usuario.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Validacion", "Ingrese la contraseña", "Aceptar");
                Password.Focus();
                return;
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://misapis.azurewebsites.net");

            var autenticacion = new Autenticacion
            {
                Usuario = usuario,
                Password = password
            };
            string json = JsonConvert.SerializeObject(autenticacion);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await client.PostAsync("/api/Seguridad", content);
            if (request.IsSuccessStatusCode)
            {
                var responseJson = await request.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Response>(responseJson);

                if (response.EsPermitido)
                {
                    await Navigation.PushAsync(new Cartelera());
                }
                else
                {
                   await DisplayAlert("Lo sentimos!", response.Mensaje, "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Lo sentimos!", "Ha ocurrido un error de comunicacion", "OK");
            }
        }
    }
}