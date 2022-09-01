namespace Oleexo.RealtimeDistributedSystem.DomainDrivenDesign;

public interface IAggregateRoot
{
    string PrintableId { get; }
    IReadOnlyCollection<IEntityEvent> GetUncommittedEvents();
    IReadOnlyCollection<IEntityError> GetUncommittedErrors();
    void ClearUncommitted();
    bool IsDirty();
    bool IsValid();
}

public interface IAggregateRoot<out TId> : IAggregateRoot
{
    TId Id { get; }
}
