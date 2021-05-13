using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using TijdlijnVisualizer.Web.Entiteiten;

namespace TijdlijnVisualizer.Web.Helpers
{
    public static class JTokenHelpers
    {
        public static Tijdlijn ToTijdlijn(this JObject obj)
        {
            var retval = new Tijdlijn();
            //ondersteunt nog niet: nested properties (behalve periode)
            foreach (var property in obj.Properties())
            {
                if (property.Name == "Periode")
                {
                    var periode = property.Children<JObject>();
                    var van = periode.Properties().First(x => x.Name == "Van");
                    var tot = periode.Properties().First(x => x.Name == "TotEnMet");
                    retval.Periode = new Periode(DateTime.Parse(van.Value.ToString()), DateTime.Parse(tot.Value.ToString()));
                }
                else
                {
                    retval.Waarden.Add(property.Name, property.Value.ToString());
                }
            }

            return retval;
        }

    }
}
