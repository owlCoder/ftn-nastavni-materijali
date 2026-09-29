using Oib.Vezba05;

var now = DateTimeOffset.UtcNow;
var operation = new RiskyOperation("Izvoz poverljivih podataka", TimeSpan.FromMinutes(10));
var staleSession = new AuthSession("ana", now.AddHours(-2), now.AddMinutes(-45), false);
var decision = new StepUpAuthenticationPolicy().Evaluate(staleSession, operation, now);

Console.WriteLine($"Dozvoljeno: {decision.Allowed}; MFA: {decision.RequiresMfa}; {decision.Reason}");
if (decision.Allowed || !decision.RequiresMfa) throw new InvalidOperationException("Stara MFA potvrda nije odbijena.");

