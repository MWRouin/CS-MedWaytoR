using MWR.MedWaytoR.PubSub;
using MWR.MedWaytoR.RequestResponse;
using WebApiMedWaytoR.Application.Abstract;
using WebApiMedWaytoR.Infrastructure;

namespace WebApiMedWaytoR.Application.Behaviour;

public class CommandBehaviourPipe<TIn, TOut>(
    IEventPublisher eventPublisher,
    InMemoryDbContext dbContext)
    : IRequestResponsePipe<TIn, TOut> where TIn : /*ICommand<TOut>*/ IRequest<TOut>
{
    public async Task<TOut> Pipe(TIn request, Func<Task<TOut>> nextPipe, CancellationToken ct)
    {
        var response = await nextPipe();

        if (request is not ICommand<TOut>) return response;

        await eventPublisher.PublishAsync(dbContext.ModifiedEntities().SelectMany(e => e.DomainEvents), ct);

        await dbContext.SaveChanges();

        return response;
    }
}