using System.Collections;
using System.Collections.Generic;

namespace TijdlijnVisualizer.Web.Helpers
{
    public static class JaarTijdlijnHelper
    {
        //in pixels
        public static int BreedteFactor = 5;
        public static int HoogteFactor = 5;
        public static int LijnBreedte = 2;
        public static int Marge = 20;
        public static int TotaleHoogte = 200;
        public static int HoogteTijdlijn = 100;

        public static IEnumerable<Maand> Maanden = new List<Maand>
        {
            new Maand
            {
                Naam = "januari",
                Label = "jan",
                Volgorde = 1,
                AantalDagen = 31
            },
            new Maand
            {
                Naam = "februari",
                Label = "feb",
                Volgorde = 2,
                AantalDagen = 28
            },
            new Maand
            {
                Naam = "maart",
                Label = "mrt",
                Volgorde = 3,
                AantalDagen = 31
            },
            new Maand
            {
                Naam = "april",
                Label = "apr",
                Volgorde = 4,
                AantalDagen = 30
            },
            new Maand
            {
                Naam = "mei",
                Label = "mei",
                Volgorde = 5,
                AantalDagen = 31
            },
            new Maand
            {
                Naam = "juni",
                Label = "jun",
                Volgorde = 6,
                AantalDagen = 30
            },
            new Maand
            {
                Naam = "juli",
                Label = "jul",
                Volgorde = 7,
                AantalDagen = 31
            },
            new Maand
            {
                Naam = "augustus",
                Label = "aug",
                Volgorde = 8,
                AantalDagen = 31
            },
            new Maand
            {
                Naam = "september",
                Label = "sep",
                Volgorde = 9,
                AantalDagen = 30
            },
            new Maand
            {
                Naam = "oktober",
                Label = "okt",
                Volgorde = 10,
                AantalDagen = 31
            },
            new Maand
            {
                Naam = "november",
                Label = "nov",
                Volgorde = 11,
                AantalDagen = 30
            },
            new Maand
            {
                Naam = "december",
                Label = "dec",
                Volgorde = 12,
                AantalDagen = 31
            }
        };
    }

    public class Maand
    {
        public string Naam { get; set; }
        public string Label { get; set; }
        public int Volgorde { get; set; }
        public int AantalDagen { get; set; }
    }
}
