
namespace ATCMediator.Mediator.Interfaces
{
    public interface IMediator
    {
        Task SendCommand<TCommand>(TCommand command);
        Task<TResult> SendQuery<TResult>(IQuery<TResult> query);
    }
}