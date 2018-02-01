using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace OttawaOpenDataExtractor
{
    /// <summary>
    /// Use open data from Ottawa to get Basketball courts information
    /// </summary>
    public class BasketBallExtractor
    {
        /// <summary>
        /// Extract the courts and also use the query the park name
        /// </summary>
        /// <returns>Tuple of longtitue, latitude and park name</returns>
        public static IEnumerable<Tuple<double, double, string>> Extract()
        {
            var basketBallClient =
                new RestClient(
                    "http://data.ottawa.ca/dataset/859ab3c4-c68c-4bf2-8ba1-1d7f3fe569e1/resource/351d2cc6-1584-4f32-b56b-dc2ea7a2a380/download/");

            var basketBallRequest = new RestRequest("basketball-courts.json", Method.GET);
            
            // execute the request
            IRestResponse baseketBallResponse = basketBallClient.Execute(basketBallRequest);

            dynamic baseketBallContent = JObject.Parse(baseketBallResponse.Content);

            var parksClient = new RestClient("http://data.ottawa.ca/api/action/");
            
            foreach (var feature in baseketBallContent.features)
            {
                var parkid = feature.properties.PARK_ID;

                var parkRequest = new RestRequest("datastore_search?resource_id=3f15f808-51db-46b5-a1e2-a19a4dc48a09&q={q}", Method.GET);

                parkRequest.AddParameter("q", "{\"PARK_ID\":\"" + parkid + "\"}", ParameterType.UrlSegment);

                IRestResponse parkResponse = parksClient.Execute(parkRequest);

                dynamic parkData = JObject.Parse(parkResponse.Content);

                var parkName = string.Empty;

                foreach (var result in parkData.result.records)
                {
                    parkName = result.NAME;
                }

                var coordinates = feature.geometry.coordinates;
                var longtitue = Convert.ToDouble(coordinates.First);
                var latitude = Convert.ToDouble(coordinates.Last);
                yield return new Tuple<double, double, string>(longtitue, latitude, parkName);
            }
        }
    }
}
