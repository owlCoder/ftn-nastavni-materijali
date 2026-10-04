using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.Application.Dispatch;

public sealed record DispatchResult(DeviceCommand Command, string Code);
