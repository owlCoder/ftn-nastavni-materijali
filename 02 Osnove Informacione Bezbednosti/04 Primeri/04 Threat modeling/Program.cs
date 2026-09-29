using Oib.Vezba04;

var exportFlow = new DataFlow("Izvoz iz internog sistema", new Asset("Lični podaci", 5), true);
var scenario = new ThreatScenario("Neovlašćen primalac izvoza", exportFlow, 3, "Potpisan zahtev i enkripcija primaoca");
var model = new ThreatModelService();

Console.WriteLine($"Rizik: {model.RiskScore(scenario)}");
Console.WriteLine($"Kontrola: {scenario.ProposedControl}");

if (!model.RequiresTreatment(scenario)) throw new InvalidOperationException("Visok rizik nije prioritizovan.");

