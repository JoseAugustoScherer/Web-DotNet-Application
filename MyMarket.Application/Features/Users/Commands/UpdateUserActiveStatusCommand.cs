using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserActiveStatusCommand(Guid Id, ActiveStatus ActiveStatus);