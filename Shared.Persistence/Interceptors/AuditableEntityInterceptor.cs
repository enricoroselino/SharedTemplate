using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Contracts.DDD;
using Shared.Persistence.Extensions;

namespace Shared.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditableProperties(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditableProperties(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableProperties(DbContext? context)
    {
        if (context is null) return;

        var entities = context.ChangeTracker
            .Entries<IEntity>()
            .ToList();

        entities.ForEach(x =>
        {
            if (x.State == EntityState.Added) x.Entity.CreatedAt = DateTime.Now;

            if (x.State == EntityState.Added || x.State == EntityState.Modified || x.HasChangedOwnedEntities())
                x.Entity.ModifiedAt = DateTime.Now;
        });
    }
}