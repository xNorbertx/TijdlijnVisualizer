using Microsoft.AspNetCore.Components;
using TijdlijnVisualizer.Web.Entiteiten;

namespace TijdlijnVisualizer.Web.Components
{
    partial class DetailOverzicht : ComponentBase
    {
        [Parameter]
        public Tijdlijn Tijdlijn { get; set; }
    }
}
