
using ATCMediator.Mediator.Interfaces;

namespace ATCMediator.Mediator
{
    public class Mediator(IServiceProvider serviceProvider) : IMediator
    {
        public async Task SendCommand<TCommand>(TCommand command)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command!.GetType());
            var handler = serviceProvider.GetService(handlerType);
            if (handler == null)
                throw new InvalidOperationException($"Handler not found for command type {command.GetType()}");

            await (Task)handlerType
                .GetMethod("Handle")!
                .Invoke(handler, new object[] { command })!;
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