namespace ATCMediator.Mediator.Interfaces;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task Handle(TCommand command);
}