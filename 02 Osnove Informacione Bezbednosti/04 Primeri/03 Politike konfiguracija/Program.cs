using Oib.Vezba03;

var baseline = new SecurityBaseline("2026.1", 14, true);
var current = new SecurityConfiguration(10, false);
var correlationId = Guid.NewGuid().ToString("N");
var findings = new ConfigurationDriftDetector().Evaluate(baseline, current);

foreach (var finding in findings)
    Console.WriteLine($"{correlationId} | {finding.Control} | expected={finding.Expected} actual={finding.Actual}");

if (findings.Count != 2) throw new InvalidOperationException("Drift nije potpuno detektovan.");

