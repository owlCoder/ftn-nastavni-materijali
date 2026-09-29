using Oib.Vezba08;

var events = new[]
{
    new SecurityEvent("corr-42", "login-failed", "ana", false),
    new SecurityEvent("corr-42", "step-up-required", "ana", true),
    new SecurityEvent("corr-42", "export-denied", "ana", true),
};

var caseFile = new EventCorrelator().Correlate(events).Single();
var measurement = new ControlMeasurement("step-up-export", events.Length, events.Count(item => item.Blocked));

Console.WriteLine($"{caseFile.CorrelationId}: {caseFile.EventCount} događaja, efektivnost {measurement.Effectiveness:P0}");
if (caseFile.EventTypes.Count != 3 || measurement.Effectiveness <= 0.5m)
    throw new InvalidOperationException("Korelacija ili merenje nije dalo očekivani rezultat.");
