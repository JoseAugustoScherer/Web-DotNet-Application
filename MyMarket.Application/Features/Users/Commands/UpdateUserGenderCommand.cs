using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserGenderCommand(Guid Id, Gender Gender);