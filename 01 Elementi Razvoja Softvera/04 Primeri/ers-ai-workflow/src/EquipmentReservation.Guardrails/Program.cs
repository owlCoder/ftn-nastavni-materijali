using EquipmentReservation.Guardrails.Parsing;
using EquipmentReservation.Guardrails.Policies;
using EquipmentReservation.Guardrails.Services;

var input = await Console.In.ReadToEndAsync();

try
{
    var invocation = HookInputParser.Parse(input);
    var evaluator = new GuardrailEvaluator([
        new SensitiveFileGuardrail(),
        new DangerousCommandGuardrail()
    ]);

    var decision = evaluator.Evaluate(invocation);
    if (decision.Allowed) return 0;

    Console.Error.WriteLine(decision.Reason);
    return 2;
}
catch (Exception exception) when (exception is not OutOfMemoryException)
{
    Console.Error.WriteLine($"Guardrail input could not be evaluated: {exception.Message}");
    return 2;
}
