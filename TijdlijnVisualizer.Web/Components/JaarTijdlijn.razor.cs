using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TijdlijnVisualizer.Web.Entiteiten;
using TijdlijnVisualizer.Web.Helpers;

namespace TijdlijnVisualizer.Web.Components
{
    partial class JaarTijdlijn : ComponentBase
    {
        [Parameter]
        public string Json { get; set; }
        
        [Parameter]
        public EventCallback<Tijdlijn> TijdlijnGeselecteerd { get; set; }

        public ICollection<Tijdlijn> Tijdlijnen { get; set; }

        public int TotaleBreedte { get; set; }
        public int TotaleHoogte { get; set; }
        public int HoogteTijdlijn { get; set; }
        public int Marge { get; set; }
        public int BreedteFactor { get; set; }
        public int LijnBreedte { get; set; }
        public int Jaar { get; set; } = DateTime.Now.Year;

        public ICollection<Tijdlijn> TmpTijdlijnen { get; set; } = new List<Tijdlijn>();


        protected override void OnInitialized()
        {
            //Initialiseer variabelen voor gebruik in dit component
            TotaleHoogte = JaarTijdlijnHelper.TotaleHoogte;
            HoogteTijdlijn = JaarTijdlijnHelper.HoogteTijdlijn;
            Marge = JaarTijdlijnHelper.Marge;
            BreedteFactor = JaarTijdlijnHelper.BreedteFactor;
            LijnBreedte = JaarTijdlijnHelper.LijnBreedte;

            //Zet de totale breedte van de SVG viewbox
            TotaleBreedte = (Marge * 2) + (365 * BreedteFactor) + (13 * LijnBreedte);

            Tijdlijnen = new List<Tijdlijn>();
            if (Json.Trim().Substring(0, 1) == "[")
            {
                var jarray = JArray.Parse(Json);
                foreach (var obj in jarray.Children<JObject>())
                {
                    Tijdlijnen.Add(obj.ToTijdlijn());
                }
            }
            else
            {
                var jobject = JObject.Parse(Json);
                Tijdlijnen.Add(jobject.ToTijdlijn());
            }
        }

        public MarkupString HtmlJaarTijdlijn()
        {
            var html = new StringBuilder();
            //horizontale lijn
            html.Append(AddSvgMarkupLijn("jaarlijn", 
                                         (Marge).ToString(), 
                                         (HoogteTijdlijn).ToString(),
                                         (Marge + (365 * BreedteFactor) + (13 * LijnBreedte)).ToString(), 
                                         (HoogteTijdlijn).ToString()));
            //verticale streepjes aan begin en eind van de lijn
            html.Append(AddSvgMarkupLijn("beginjaar", 
                                         (Marge).ToString(), 
                                         (HoogteTijdlijn - BreedteFactor).ToString(), 
                                         (Marge).ToString(), 
                                         (HoogteTijdlijn + BreedteFactor).ToString()));
            html.Append(AddSvgMarkupLijn("eindjaar",
                                         (Marge + (365 * BreedteFactor) + (12 * LijnBreedte) + (LijnBreedte / 2)).ToString(), 
                                         (HoogteTijdlijn - BreedteFactor).ToString(),
                                         (Marge + (365 * BreedteFactor) + (12 * LijnBreedte) + (LijnBreedte / 2)).ToString(), 
                                         (HoogteTijdlijn + BreedteFactor).ToString()));

            //verticale streepjes per maand en labeltjes per maand
            int aantalDagen = 0;
            foreach (var maand in JaarTijdlijnHelper.Maanden.OrderBy(x => x.Volgorde))
            {
                var xPosLabel = Marge + (aantalDagen * BreedteFactor) + (maand.AantalDagen * BreedteFactor / 2) + (maand.Volgorde * LijnBreedte);
                html.Append(AddSvgMarkupMaandLabel(maand.Naam, maand.Label, xPosLabel.ToString(), (HoogteTijdlijn + (BreedteFactor * 3)).ToString()));

                aantalDagen += maand.AantalDagen;

                html.Append(AddSvgMarkupLijn(maand.Naam,
                                             (Marge + (aantalDagen * BreedteFactor) + (maand.Volgorde * LijnBreedte) + (LijnBreedte / 2)).ToString(),
                                             (HoogteTijdlijn).ToString(),
                                             (Marge + (aantalDagen * BreedteFactor) + (maand.Volgorde * LijnBreedte) + (LijnBreedte / 2)).ToString(),
                                             (HoogteTijdlijn + BreedteFactor).ToString()));


            }
            return new MarkupString(html.ToString());
        }

        private string AddSvgMarkupLijn(string id, string x1, string y1, string x2, string y2)
        {
            return $"<line id=\"l-{id}\" " +
                   $"x1 =\"{x1}\" " +
                   $"y1=\"{y1}\" " +
                   $"x2=\"{x2}\" " +
                   $"y2=\"{y2}\" " +
                   $"stroke=\"black\" stroke-width=\"{LijnBreedte}\"></line>";
        }

        private string AddSvgMarkupMaandLabel(string id, string label, string x, string y)
        {
            return $"<text id=\"lbl-{id}\" " +
                   $"class=\"small\" " +
                   $"x =\"{x}\" " +
                   $"y=\"{y}\">{label}</text>";
        }

        public async Task ToonInfo(MouseEventArgs e, Tijdlijn tijdlijn)
        {
            TmpTijdlijnen.Clear();
            await TijdlijnGeselecteerd.InvokeAsync(tijdlijn);
        }

        public int GetAantalTijdlijnenMetOverlap(Tijdlijn tijdlijn)
        {
            return TmpTijdlijnen.Count(x => x.Periode.HeeftOverlapMet(tijdlijn.Periode));
        }
    }
}
