namespace MyMarket.Application.Features.Users.Commands;

public record UpdateUserIncreaseAmountCommand(Guid Id, decimal Amount);