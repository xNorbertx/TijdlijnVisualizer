using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TijdlijnVisualizer.Web.Entiteiten
{
    public class Periode
    {
        public DateTime Van { get; set; }
        public DateTime TotEnMet { get; set; }


        public Periode(DateTime van, DateTime totEnMet)
        {
            Van = van;
            TotEnMet = totEnMet;
        }


        public override string ToString()
        {
            return $"{Van:dd-MM-yyyy} t/m {TotEnMet:dd-MM-yyyy}";
        }
    }

    public static class PeriodeExtensions
    {
        public static Periode Doorsnede(this ICollection<Periode> periodes)
        {
            var maxVan = periodes.Where(periode => periode != null).Select(x => x.Van).Max();
            var minTotEnMet = periodes.Where(periode => periode != null).Select(x => x.TotEnMet).Min();

            return maxVan <= minTotEnMet ? new Periode(maxVan, minTotEnMet) : null;
        }

        public static Periode Doorsnede(this Periode periode, Periode periode2)
        {
            return new List<Periode> { periode, periode2 }.Doorsnede();
        }

        public static bool HeeftOverlapMet(this Periode deze, Periode andere)
        {
            return deze.Doorsnede(andere) != null;
        }

        public static bool HeeftOverlapMetJaar(this Periode deze, int jaar)
        {
            var jaarPeriode = new Periode(new DateTime(jaar, 1, 1), new DateTime(jaar, 12, 31));
            return deze.HeeftOverlapMet(jaarPeriode);
        }

        public static bool IsVolledigIn(this Periode deze, int jaar)
        {
            var jaarPeriode = new Periode(new DateTime(jaar, 1, 1), new DateTime(jaar, 12, 31));
            return IsVolledigIn(deze, jaarPeriode);
        }

        public static bool IsVolledigIn(this Periode deze, Periode jaarPeriode)
        {
            return deze.Van >= jaarPeriode.Van
                && deze.TotEnMet <= jaarPeriode.TotEnMet;
        }
    }
}
