namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserPasswordCommand(Guid Id, string Password);