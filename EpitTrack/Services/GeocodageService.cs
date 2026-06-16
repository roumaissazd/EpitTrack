using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

public interface IGeocodageService
{
    Task<(string, string)> ObtenirCoordonnees(string adresse);
}

public class GeocodageService : IGeocodageService
{
    private readonly HttpClient _client;

    public GeocodageService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new System.Uri("https://api-adresse.data.gouv.fr");
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }



    public class Geometry
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    public class Properties
    {
        public string Label { get; set; }
        public double Score { get; set; }
        public string Housenumber { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Postcode { get; set; }
        public string Citycode { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string City { get; set; }
        public string Context { get; set; }
        public string Type { get; set; }
        public double Importance { get; set; }
        public string Street { get; set; }
    }

    public class Feature
    {
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
        public Properties Properties { get; set; }
    }

    public class MyGeoData
    {
        public string Type { get; set; }
        public string Version { get; set; }
        public List<Feature> Features { get; set; }
        public string Attribution { get; set; }
        public string Licence { get; set; }
        public string Query { get; set; }
        public int Limit { get; set; }
    }

    public async Task<(string, string)> ObtenirCoordonnees(string adresse)
    {
        string latitude = "";
        string longitude = "";
        /***** verifier s'il s'agit d'un PI**/

        if (adresse.ToUpper().Contains("ROPORT") && adresse.ToUpper().Contains("GAULLE"))
            {
              latitude = "49.0032309";
              longitude = "2.5743366";
            }
        else
            if (adresse.ToUpper().Contains("ROPORT") && adresse.ToUpper().Contains("ORLY"))
            {
            latitude = "48.7283333333";
            longitude = "2.3658333333";
            }
             else
             if (adresse.ToUpper().Contains("GARE") && adresse.ToUpper().Contains("MONTPARNASSE"))
                {
                   latitude = "48.840670";
                   longitude = "2.319340";

                }
               else
                if (adresse.ToUpper().Contains("GARE") && adresse.ToUpper().Contains("LAZARE"))
                   {
                     latitude = "48,8763";
                     longitude = "2,3254";

                   }
                else
                if (adresse.ToUpper().Contains("GARE") && adresse.ToUpper().Contains("NORD"))
                  {
                    latitude = "48,880931";
                    longitude = "2,355323";
                  }
                  else
                  if (adresse.ToUpper().Contains("GARE") && adresse.ToUpper().Contains("AUSTERLITZ"))
                     {
                       latitude = "48,842626";
                       longitude = "2,364971";
                     }
                    else
                    {
            try
            { 
              MyGeoData GeoResult;
              var test = System.Uri.EscapeDataString(adresse);
              HttpResponseMessage response = await _client.GetAsync($"/search/?q={System.Uri.EscapeDataString(adresse)}");
              var jstest = await response.Content.ReadAsStringAsync();
              GeoResult = JsonConvert.DeserializeObject<MyGeoData>(jstest);
              if (GeoResult.Features.Count() > 0 ) 
                { 
                  longitude = GeoResult.Features.FirstOrDefault().Geometry.Coordinates[0].ToString();
                  latitude = GeoResult.Features.FirstOrDefault().Geometry.Coordinates[1].ToString();
               }
              
            }
            catch (FormatException)
            {

            }
        }
       return (latitude, longitude);
    }
}
