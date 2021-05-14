using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TijdlijnVisualizer.Web.Services;

namespace TijdlijnVisualizer.Web.Pages
{
    partial class Invoer : ComponentBase
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        ITijdlijnService TijdlijnService { get; set; }

        public string InvoerText { get; set; }
        public string OngeldigeInvoer { get; set; } = string.Empty;

        protected override void OnInitialized()
        {
            TijdlijnService.EmptyTijdlijnen();
        }

        public void ValideerInvoer()
        {
            try
            {
                var jtoken = JToken.Parse(InvoerText);

                if (PeriodeAanwezig())
                {
                    var jobjecten = new List<JObject>();
                    if (InvoerText.Trim().Substring(0, 1) == "[")
                    {
                        var jarray = JArray.Parse(InvoerText);
                        foreach (var obj in jarray.Children<JObject>())
                        {
                            jobjecten.Add(obj);
                        }
                    }
                    else
                    {
                        var jobject = JObject.Parse(InvoerText);
                        jobjecten.Add(jobject);
                    }
                    TijdlijnService.SetTijdlijnen(jobjecten);
                    NavigationManager.NavigateTo($"Overzicht");
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
