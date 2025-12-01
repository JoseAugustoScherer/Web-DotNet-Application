namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserNameCommand(Guid Id, string Name);