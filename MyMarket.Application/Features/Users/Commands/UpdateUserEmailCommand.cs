namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserEmailCommand(Guid Id, string Email);