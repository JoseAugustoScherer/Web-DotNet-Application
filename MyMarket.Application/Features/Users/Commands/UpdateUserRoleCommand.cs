using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserRoleCommand(Guid Id, Role Role);