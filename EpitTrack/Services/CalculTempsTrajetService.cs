using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace EpitTrack.Services
{
    public interface ICalculTempsTrajetService
    {
        Task<int> ObtenirTempsTrajetEntreDeuxPoints(string departLatitude, string departLongitude, string arriveeLatitude, string arriveeLongitude);
    }

    public class CalculTempsTrajetService : ICalculTempsTrajetService
    {
        private readonly HttpClient _client;

        public CalculTempsTrajetService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://router.project-osrm.org/route/v1/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<int> ObtenirTempsTrajetEntreDeuxPoints(string departLatitude, string departLongitude, string arriveeLatitude, string arriveeLongitude)
        {
            if (!string.IsNullOrEmpty(departLongitude) && !string.IsNullOrEmpty(departLatitude) && !string.IsNullOrEmpty(arriveeLongitude) && !string.IsNullOrEmpty(arriveeLatitude))
            {
                string coordinates = departLongitude.Replace(",", ".") + "," + departLatitude.Replace(",", ".") + ";" + arriveeLongitude.Replace(",", ".") + "," + arriveeLatitude.Replace(",", ".");
                HttpResponseMessage response = await _client.GetAsync($"driving/{coordinates}");
                var jstest = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonResponse);
                    int tempsTrajet = jsonObject["routes"][0]["duration"].Value<int>();
                    return tempsTrajet;
                }
                else
                {
                    return 0;

                }
            }
            else
            {
                return 0;
            }
        }

    }
}
