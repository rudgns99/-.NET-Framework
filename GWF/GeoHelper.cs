using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System;

namespace GWF
{
    public class GeoHelper
    {
        private readonly static string APIURL = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&language={2}&key={3}";

        public static object GeoCodeString { get; private set; }

        public static async Task<List<GeoResult>> GetAddress(object LATITUDE, object LONGITUDE)
        {
            return await GetAddress(LATITUDE, LONGITUDE, "en", "AIzaSyB4MASL5OQOl83IdcicGG9Du_YtibWbkP4");
        }

        public static async Task<List<GeoResult>> GetAddress(object LATITUDE, object LONGITUDE, string LANG, string APIKEY)
        {
            using (WebClient wc = new WebClient())
            {                
                wc.Encoding = Encoding.UTF8;
                wc.Timeout = 600 * 60 * 1000;
                string GeoCodeString = await wc.DownloadStringTaskAsync(string.Format(APIURL, LATITUDE, LONGITUDE, LANG, APIKEY));
                GeoRoot geo = JsonHelper.Deserialize<GeoRoot>(GeoCodeString.Trim());
                if (geo.status.Equals("OK") && geo.results.Count > 0)
                {
                    return geo.results;
                }
                else
                    return null;
            }
        }

        public static async Task<string> GetOnlyAddress(object LATITUDE, object LONGITUDE)
        {
            List<GeoResult> List = await GetAddress(LATITUDE, LONGITUDE);
            if (List.Count > 0)
                return List[0].formatted_address;
            else
                return null;
        }

        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }

        public class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Bounds
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Northeast2
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest2
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Viewport
        {
            public Northeast2 northeast { get; set; }
            public Southwest2 southwest { get; set; }
        }

        public class Geometry
        {
            public Bounds bounds { get; set; }
            public Location location { get; set; }
            public string location_type { get; set; }
            public Viewport viewport { get; set; }
        }

        public class GeoResult
        {
            public List<AddressComponent> address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public List<string> types { get; set; }
        }

        public class GeoRoot
        {
            public List<GeoResult> results { get; set; }
            public string status { get; set; }
        }
    }
}
