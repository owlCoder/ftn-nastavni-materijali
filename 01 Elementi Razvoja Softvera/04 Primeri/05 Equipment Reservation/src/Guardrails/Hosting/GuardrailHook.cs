using EquipmentReservation.Guardrails.Interfaces;

namespace EquipmentReservation.Guardrails.Hosting;

public sealed class GuardrailHook
{
    private readonly IToolInvocationParser _parser;
    private readonly IGuardrailEvaluator _evaluator;

    public GuardrailHook(IToolInvocationParser parser, IGuardrailEvaluator evaluator)
    {
        _parser = parser;
        _evaluator = evaluator;
    }

    public async Task<int> RunAsync(TextReader input, TextWriter error)
    {
        try
        {
            var invocation = _parser.Parse(await input.ReadToEndAsync());
            if (!invocation.Valid)
            {
                await error.WriteLineAsync("Guardrail input could not be evaluated.");
                return HookExitCodes.Block;
            }

            var decision = _evaluator.Evaluate(invocation);
            if (decision.Allowed)
                return HookExitCodes.Allow;

            await error.WriteLineAsync(decision.Reason);
            return HookExitCodes.Block;
        }
        catch (Exception exception) when (exception is not OutOfMemoryException)
        {
            await error.WriteLineAsync($"Guardrail input could not be evaluated: {exception.Message}");
            return HookExitCodes.Block;
        }
    }
}
