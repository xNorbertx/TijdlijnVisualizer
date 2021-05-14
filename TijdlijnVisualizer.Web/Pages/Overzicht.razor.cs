using Microsoft.AspNetCore.Components;
using TijdlijnVisualizer.Web.Entiteiten;

namespace TijdlijnVisualizer.Web.Pages
{
    partial class Overzicht : ComponentBase
    {
        [Inject]
        public NavigationManager NavManager { get; set; }

        public Tijdlijn GeselecteerdTijdlijn { get; set; } = null;

        public void ZetDetailOverzicht(Tijdlijn tijdlijn)
        {
            GeselecteerdTijdlijn = tijdlijn;
        }

        public void NavNaarInvoer()
        {
            NavManager.NavigateTo("/");
            return;
        }
    }
}
