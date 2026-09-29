using Oib.Vezba06;

var now = DateTimeOffset.UtcNow;
var attempts = Enumerable.Range(1, 5).Select(_ => new LoginAttempt("ana", "203.0.113.10", false, now)).ToArray();
var signal = new FailedLoginDetector().Detect(attempts, 5) ?? throw new InvalidOperationException("Signal nije detektovan.");
var incident = new IncidentFactory().Open(signal);

Console.WriteLine($"{incident.Id} | {incident.Severity} | {incident.Summary} | {incident.Status}");

