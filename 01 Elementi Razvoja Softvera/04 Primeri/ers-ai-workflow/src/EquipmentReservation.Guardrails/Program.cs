using EquipmentReservation.Guardrails.Hosting;
using EquipmentReservation.Guardrails.Parsing;
using EquipmentReservation.Guardrails.Policies;
using EquipmentReservation.Guardrails.Services;

var hook = new GuardrailHook(
    new HookInputParser(),
    new GuardrailEvaluator([
        new SensitiveFileGuardrail(),
        new DangerousCommandGuardrail()
    ]));

return await hook.RunAsync(Console.In, Console.Error);
