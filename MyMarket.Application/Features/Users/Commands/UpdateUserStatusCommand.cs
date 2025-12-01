using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserStatusCommand(Guid Id, ActiveStatus ActiveStatus);