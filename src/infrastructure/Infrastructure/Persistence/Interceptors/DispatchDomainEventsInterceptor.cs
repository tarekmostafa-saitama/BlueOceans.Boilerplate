using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DispatchDomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var savingResult = base.SavingChanges(eventData, result);
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();

        return savingResult;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var savingResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        await DispatchDomainEvents(eventData.Context);
        return savingResult;
    }

    public async Task DispatchDomainEvents(DbContext context)
    {
        if (context == null) return;

        var entities = context.ChangeTracker
            .Entries<Aggregate<object>>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var baseEntities = entities.ToList();
        var domainEvents = baseEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        baseEntities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }
}