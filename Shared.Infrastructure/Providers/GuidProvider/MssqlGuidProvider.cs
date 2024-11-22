using UUIDNext;

namespace Shared.Infrastructure.Providers.GuidProvider;

public class MssqlGuidProvider : IGuidProvider
{
    public Guid NewRandom() => Guid.NewGuid();
    public Guid NewSequential() => Uuid.NewDatabaseFriendly(Database.SqlServer);
}