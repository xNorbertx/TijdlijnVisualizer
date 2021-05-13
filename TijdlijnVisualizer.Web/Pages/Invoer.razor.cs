using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using TijdlijnVisualizer.Web.Services;

namespace TijdlijnVisualizer.Web.Pages
{
    partial class Invoer : ComponentBase
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }

        //[Inject]
        //TijdlijnService TijdlijnService { get; set; }

        public string InvoerText { get; set; } = "Vul hier je JSON in.";
        public string OngeldigeInvoer { get; set; } = string.Empty;

        public void ValideerInvoer()
        {
            try
            {
                var jtoken = JToken.Parse(InvoerText);

                if (PeriodeAanwezig())
                {
                    NavigationManager.NavigateTo($"Overzicht?JsonInvoer={InvoerText}");
                    return;
                }

                OngeldigeInvoer = "Voer een JSON string met een periode in.";
            }
            catch (JsonReaderException)
            {
                OngeldigeInvoer = "Voer een geldige JSON string in.";
            }
        }

        public void TextEvent(ChangeEventArgs args)
        {
            InvoerText = args.Value.ToString();
            OngeldigeInvoer = string.Empty;
        }

        public bool PeriodeAanwezig()
        {
            return InvoerText.Contains("Periode", StringComparison.InvariantCultureIgnoreCase)
                && InvoerText.Contains("Van", StringComparison.InvariantCultureIgnoreCase)
                && InvoerText.Contains("Tot", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
