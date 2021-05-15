using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TijdlijnVisualizer.Web.Entiteiten
{
    public class Tijdlijn
    {
        public Periode Periode { get; set; }
        public IDictionary<string, string> Waarden { get; set; }
        public int Rij { get; set; }

        public Tijdlijn()
        {
            Waarden = new Dictionary<string, string>();
        }
    }

    public static class TijdlijnExtensions
    {
        public static ICollection<Tijdlijn> SplitsOpJaarGrens(this Tijdlijn tijdlijn)
        {
            var retval = new List<Tijdlijn>();
            if (tijdlijn.Periode.Van.Year != tijdlijn.Periode.TotEnMet.Year)
            {
                var aantalJaren = tijdlijn.Periode.TotEnMet.Year - tijdlijn.Periode.Van.Year;
                //voor een verschil van 1 jaar moeten er twee perioden komen
                for (var i = 0; i < aantalJaren + 1; i++)
                {
                    var vanDatum = i == 0
                        ? tijdlijn.Periode.Van
                        : new DateTime(tijdlijn.Periode.Van.Year + i, 1, 1);
                    var totEnMetDatum = i == aantalJaren
                        ? tijdlijn.Periode.TotEnMet
                        : new DateTime(tijdlijn.Periode.Van.Year + i, 12, 31);
                    retval.Add(new Tijdlijn
                    {
                        Waarden = tijdlijn.Waarden,
                        Periode = new Periode(vanDatum, totEnMetDatum)
                    });
                }
            }
            else
            {
                retval.Add(tijdlijn);
            }
            return retval;
        }

        public static ICollection<Tijdlijn> SplitsOpJaargrens(this ICollection<Tijdlijn> tijdlijnen)
        {
            var retval = new List<Tijdlijn>();
            foreach (var tijdlijn in tijdlijnen)
            {
                retval.AddRange(tijdlijn.SplitsOpJaarGrens());       
            }


            return retval;
        }

        public static TijdlijnPositie BepaaldTijdlijnPositie(this Tijdlijn tijdlijn, int jaar)
        {
            if (tijdlijn.Periode.Van.Year == jaar && tijdlijn.Periode.TotEnMet.Year == jaar)
            {
                return TijdlijnPositie.VolledigInJaar;
            }

            if (tijdlijn.Periode.Van.Year < jaar && tijdlijn.Periode.TotEnMet.Year == jaar)
            {
                return TijdlijnPositie.DeelsInVorigJaar;
            }

            if (tijdlijn.Periode.Van.Year == jaar && tijdlijn.Periode.TotEnMet.Year > jaar)
            {
                return TijdlijnPositie.DeelsInVolgendJaar;
            }

            return TijdlijnPositie.DeelsInVorigEnVolgendJaar;
        }
    }

    public enum TijdlijnPositie
    {
        VolledigInJaar,
        DeelsInVorigJaar,
        DeelsInVolgendJaar,
        DeelsInVorigEnVolgendJaar
    }
}

