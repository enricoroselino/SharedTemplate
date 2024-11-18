namespace Shared.Contracts.DDD;

public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}