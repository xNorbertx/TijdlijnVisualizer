using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using TijdlijnVisualizer.Web.Entiteiten;

namespace TijdlijnVisualizer.Web.Pages
{
    partial class Overzicht : ComponentBase
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string JsonInvoer { get; set; }

        public Tijdlijn GeselecteerdTijdlijn { get; set; } = null;

        public void ZetDetailOverzicht(Tijdlijn tijdlijn)
        {
            GeselecteerdTijdlijn = tijdlijn;
        }

        protected override void OnInitialized()
        {
            var query = new Uri(NavigationManager.Uri).Query;
            var decodedJson = query.Substring("?jsoninvoer=".Length);
            JsonInvoer = Uri.UnescapeDataString(decodedJson);
        }
    }
}
