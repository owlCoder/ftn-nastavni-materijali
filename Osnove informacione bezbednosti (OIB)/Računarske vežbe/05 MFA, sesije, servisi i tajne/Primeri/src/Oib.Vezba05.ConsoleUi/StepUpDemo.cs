using Oib.Vezba05.Application.StepUp;

namespace Oib.Vezba05.ConsoleUi;

public sealed class StepUpDemo(IAuthorizeRiskyOperationUseCase authorizeOperation, TextWriter output)
{
    public void Run()
    {
        output.WriteLine($"Operacija: {DemoData.ConfidentialExport.Name}");

        Show("Sveža MFA potvrda", DemoData.FreshSessionId);
        Show("Stara MFA potvrda", DemoData.StaleSessionId);
        Show("Samo lozinka", DemoData.PasswordOnlySessionId);
        Show("Opozvana sesija", DemoData.RevokedSessionId);
        Show("Nepoznata sesija", "sesija-ne-postoji");
    }

    private void Show(string label, string sessionId)
    {
        var decision = authorizeOperation.Authorize(
            new AuthorizeOperationRequest(sessionId, DemoData.ConfidentialExport));

        output.WriteLine(
            $"{label}: dozvoljeno={decision.Allowed}; traži MFA={decision.RequiresMfa}; " +
            $"{decision.Reason}");
    }
}
