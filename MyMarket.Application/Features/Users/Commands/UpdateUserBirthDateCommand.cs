namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserBirthDateCommand(Guid Id, DateTime BirthDate);