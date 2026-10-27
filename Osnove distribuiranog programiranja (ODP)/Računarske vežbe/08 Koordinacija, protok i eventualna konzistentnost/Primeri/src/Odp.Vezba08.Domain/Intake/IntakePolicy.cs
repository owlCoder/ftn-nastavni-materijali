namespace Odp.Vezba08.Domain.Intake;

public sealed class IntakePolicy
{
    private readonly int _capacity;

    public IntakePolicy(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 1);
        _capacity = capacity;
    }

    /// <summary>Kada je red pun, pošiljalac dobija jasan odgovor umesto da posao tiho čeka.</summary>
    public string Decide(int queuedJobs) =>
        queuedJobs < _capacity ? IntakeCodes.Accepted : IntakeCodes.Overloaded;
}
