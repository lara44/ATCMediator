
using ATCMediator.Mediator.Interfaces;

namespace ATCMediator.Mediator
{
    public class Mediator(IServiceProvider serviceProvider) : IMediator
    {
        public async Task<TResult> SendCommand<TResult>(ICommand<TResult> command)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = serviceProvider.GetService(handlerType)
                            ?? throw new InvalidOperationException($"No se encontró handler para {handlerType.Name}");

            return await handler.Handle((dynamic)command);
        }

        public async Task<TResult> SendQuery<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = serviceProvider.GetService(handlerType)
                            ?? throw new InvalidOperationException($"No se encontró handler para {handlerType.Name}");
            return await handler.Handle((dynamic)query);
        }
    }
}