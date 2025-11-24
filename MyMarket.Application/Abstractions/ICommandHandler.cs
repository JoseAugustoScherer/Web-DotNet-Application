namespace MyMarket.Application.Abstractions;

public interface ICommandHandler<TCommand>
{
    Task HandleAsync(TCommand command);
}

public interface ICommandHandler<TCommand, TResponse>
{
    Task<TResponse> HandleAsync(TCommand command);
}