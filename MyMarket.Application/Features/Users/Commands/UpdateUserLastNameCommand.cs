namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserLastNameCommand(Guid Id, string LastName);