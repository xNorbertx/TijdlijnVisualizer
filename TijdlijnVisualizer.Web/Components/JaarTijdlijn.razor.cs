using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TijdlijnVisualizer.Web.Entiteiten;
using TijdlijnVisualizer.Web.Helpers;
using TijdlijnVisualizer.Web.Services;

namespace TijdlijnVisualizer.Web.Components
{
    partial class JaarTijdlijn : ComponentBase
    {
        [Inject]
        public ITijdlijnService TijdlijnService { get; set; }
        
        [Parameter]
        public EventCallback<Tijdlijn> TijdlijnGeselecteerd { get; set; }

        
        public int TotaleBreedte { get; set; }
        public int TotaleHoogte { get; set; }
        public int HoogteTijdlijn { get; set; }
        public int Marge { get; set; }
        public int BreedteFactor { get; set; }
        public int HoogteFactor { get; set; }
        public int LijnBreedte { get; set; }
        public int Jaar { get; set; }
        public bool IsSchrikkelJaar { get; set; }

        public ICollection<Tijdlijn> Tijdlijnen { get; set; }
        public IEnumerable<Tijdlijn> TijdlijnenInJaar { get; set; }
        public ICollection<Tijdlijn> TijdlijnenGeplaatst { get; set; } = new List<Tijdlijn>();


        protected override void OnInitialized()
        {
            //Initialiseer tijdlijnen in dit jaar
            Jaar = DateTime.Now.Year;
            IsSchrikkelJaar = Jaar % 4 == 0;
            Tijdlijnen = TijdlijnService.GetTijdlijnen();
            //Tijdlijnen = Tijdlijnen.SplitsOpJaargrens();
            TijdlijnenInJaar = Tijdlijnen.Where(x => x.Periode.HeeftOverlapMetJaar(Jaar));

            //Initialiseer variabelen voor gebruik in dit component
            TotaleHoogte = JaarTijdlijnHelper.TotaleHoogte;
            HoogteTijdlijn = JaarTijdlijnHelper.HoogteTijdlijn;
            Marge = JaarTijdlijnHelper.Marge;
            BreedteFactor = JaarTijdlijnHelper.BreedteFactor;
            HoogteFactor = JaarTijdlijnHelper.HoogteFactor;
            LijnBreedte = JaarTijdlijnHelper.LijnBreedte;

            //Zet de totale breedte van de SVG viewbox
            TotaleBreedte = (Marge * 2) + (365 * BreedteFactor) + (13 * LijnBreedte);
        }

        public MarkupString HtmlJaarTijdlijn()
        {
            if (IsSchrikkelJaar)
            {
                JaarTijdlijnHelper.Maanden.First(x => x.Naam == "februari").AantalDagen = 29;
            }

            var html = new StringBuilder();
            //horizontale lijn
            html.Append(AddSvgMarkupLijn("jaarlijn", 
                                         (Marge).ToString(), 
                                         (HoogteTijdlijn).ToString(),
                                         (Marge + (365 * BreedteFactor) + (13 * LijnBreedte)).ToString(), 
                                         (HoogteTijdlijn).ToString()));
            //verticale streepjes aan begin en eind van de lijn
            html.Append(AddSvgMarkupLijn("beginjaar", 
                                         (Marge + (LijnBreedte / 2)).ToString(), 
                                         (HoogteTijdlijn - BreedteFactor).ToString(), 
                                         (Marge + (LijnBreedte / 2)).ToString(), 
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
            TijdlijnenGeplaatst.Clear();
            await TijdlijnGeselecteerd.InvokeAsync(tijdlijn);
        }

        public int GetAantalTijdlijnenMetOverlap(Tijdlijn tijdlijn)
        {
            return TijdlijnenGeplaatst.Count(x => x.Periode.HeeftOverlapMet(tijdlijn.Periode));
        }

        public bool HeeftOverlapOpDezeRij(int rij, Tijdlijn tijdlijn)
        {
            var tijdlijnenOpRij = TijdlijnenGeplaatst.Where(x => x.Rij == rij);
            return tijdlijnenOpRij.Any(x => x.Periode.HeeftOverlapMet(tijdlijn.Periode));
        }

        public void NaarVorigJaar()
        {
            TijdlijnenGeplaatst.Clear();
            Jaar--;
            IsSchrikkelJaar = Jaar % 4 == 0;
            TijdlijnenInJaar = Tijdlijnen.Where(x => x.Periode.HeeftOverlapMetJaar(Jaar));
        }

        public void NaarVolgendJaar()
        {
            TijdlijnenGeplaatst.Clear();
            Jaar++;
            IsSchrikkelJaar = Jaar % 4 == 0;
            TijdlijnenInJaar = Tijdlijnen.Where(x => x.Periode.HeeftOverlapMetJaar(Jaar));
        }

        public TijdlijnPositie BepaalTijdlijnPositie(Tijdlijn tijdlijn)
        {
            return tijdlijn.BepaaldTijdlijnPositie(Jaar);
        }
    }
}
