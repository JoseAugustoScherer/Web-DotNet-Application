namespace MyMarket.Application.Features.Users.Commands;

public sealed record LoginUserCommand(string Email, string Password);