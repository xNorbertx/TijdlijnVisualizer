using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TijdlijnVisualizer.Web.Entiteiten;
using TijdlijnVisualizer.Web.Helpers;

namespace TijdlijnVisualizer.Web.Services
{
    public class TijdlijnService : ITijdlijnService
    {
        private ICollection<Tijdlijn> Tijdlijnen { get; set; } = new List<Tijdlijn>();

        public ICollection<Tijdlijn> GetTijdlijnen()
        {
            return Tijdlijnen;
        }

        public void SetTijdlijnen(IEnumerable<JObject> objecten)
        {
            foreach (var obj in objecten)
            {
                Tijdlijnen.Add(obj.ToTijdlijn());
            }
        }
    }
}
