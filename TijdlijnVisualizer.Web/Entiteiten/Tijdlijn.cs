using System.Collections.Generic;

namespace TijdlijnVisualizer.Web.Entiteiten
{
    public class Tijdlijn
    {
        public Periode Periode { get; set; }
        public IDictionary<string, string> Waarden { get; set; }

        public Tijdlijn()
        {
            Waarden = new Dictionary<string, string>();
        }
    }
}
