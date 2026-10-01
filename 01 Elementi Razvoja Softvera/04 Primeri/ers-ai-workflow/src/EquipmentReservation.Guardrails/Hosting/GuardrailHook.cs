using EquipmentReservation.Guardrails.Abstractions;

namespace EquipmentReservation.Guardrails.Hosting;

public sealed class GuardrailHook(IToolInvocationParser parser, IGuardrailEvaluator evaluator)
{
    public async Task<int> RunAsync(TextReader input, TextWriter error)
    {
        try
        {
            var invocation = parser.Parse(await input.ReadToEndAsync());
            var decision = evaluator.Evaluate(invocation);
            if (decision.Allowed)
                return HookExitCodes.Allow;

            await error.WriteLineAsync(decision.Reason);
            return HookExitCodes.Block;
        }
        catch (Exception exception) when (exception is not OutOfMemoryException)
        {
            await error.WriteLineAsync(
                $"Guardrail input could not be evaluated: {exception.Message}");
            return HookExitCodes.Block;
        }
    }
}
