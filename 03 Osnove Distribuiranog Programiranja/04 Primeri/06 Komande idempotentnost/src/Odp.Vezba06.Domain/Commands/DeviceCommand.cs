namespace Odp.Vezba06.Domain.Commands;

/// <summary>Namera sa stabilnim identifikatorom; ponovno slanje nosi isti CommandId.</summary>
public sealed record DeviceCommand(
    string CommandId,
    string DeviceId,
    string Action,
    CommandState State,
    int Attempts,
    DateTimeOffset LastSentAt);
