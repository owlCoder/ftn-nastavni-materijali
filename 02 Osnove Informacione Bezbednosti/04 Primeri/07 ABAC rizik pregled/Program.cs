using Oib.Vezba07;

var policy = new RiskBasedAccessPolicy();
var safe = policy.Evaluate(new("Operator", true, "office", true, 20), DateTimeOffset.UtcNow);
var risky = policy.Evaluate(new("Operator", false, "unknown", true, 85), DateTimeOffset.UtcNow);

Console.WriteLine($"Upravljani uređaj: {safe.Allowed} — {safe.Reason}");
Console.WriteLine($"Nepoznat uređaj: {risky.Allowed} — {risky.Reason}");
if (!safe.Allowed || risky.Allowed) throw new InvalidOperationException("ABAC odluke nisu očekivane.");

