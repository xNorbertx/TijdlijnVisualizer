using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TijdlijnVisualizer.Web.Entiteiten;

namespace TijdlijnVisualizer.Web.Services
{
    public interface ITijdlijnService
    {
        ICollection<Tijdlijn> GetTijdlijnen();
        void SetTijdlijnen(IEnumerable<JObject> objecten);
    }
}
