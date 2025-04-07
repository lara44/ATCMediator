
namespace ATCMediator.Mediator.Interfaces
{
    public interface IMediator
    {
        Task<TResult> SendCommand<TResult>(ICommand<TResult> command);
        Task<TResult> SendQuery<TResult>(IQuery<TResult> query);
    }
}