# TijdlijnVisualizer

Eigenschappen: 
- Blazor Server-side applicatie
- Alle visualisatie door middel van SVG

Werking:
- Voer JSON in een textarea. Als deze een periode bevat wordt deze gevisualiseerd in een tijdlijn.
- De overige attributen van de JSON invoer zijn te zien wanneer de tijdlijn wordt geselecteerd.

Voorbeeldinvoer:
[
	{
		"Soort": "tijdlijn",
		"Hoeveelheid": 1900,
		"Periode": {
			"Van": "2021-01-01",
			"TotEnMet": "2021-01-31"
		}
	},
	{
		"Type": "tijdlijn",
		"Hoeveelheid": 3.0,
		"Periode": {
			"Van": "2020-12-01",
			"TotEnMet": "2021-01-31"
		}
	}
]	


To do:
- Tijdlijnen over meerdere jaren visualiseren + omgaan met schrikkeljaren
- Invoer door middel van een bestand (drag & drop)
- Algehele styling opkrikken
- Verfijning van validatie op JSON invoer
- Unit testen toevoegen
- Ergens online hosten (waarschijnlijk Azure WebApp)
