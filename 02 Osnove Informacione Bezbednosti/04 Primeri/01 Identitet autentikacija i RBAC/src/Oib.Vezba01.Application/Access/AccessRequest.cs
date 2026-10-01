using Oib.Vezba01.Domain.Identity;

namespace Oib.Vezba01.Application.Access;

public sealed record AccessRequest(
    Actor Actor,
    string Permission);
