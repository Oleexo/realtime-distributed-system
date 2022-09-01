namespace Oleexo.RealtimeDistributedSystem.DomainDrivenDesign;

public abstract class BaseAggregateRoot<T>
    : IAggregateRoot
    where T : notnull {
    private readonly ICollection<IEntityError> _errors;
    private readonly ICollection<IEntityEvent> _events;

    protected BaseAggregateRoot(T id) {
        Id = id;
        _errors = new List<IEntityError>();
        _events = new List<IEntityEvent>();
    }

    public T Id { get; }

    public string PrintableId => Id.ToString() ?? throw new NullReferenceException("Id should ne be null");

    public bool IsValid() {
        return !_errors.Any();
    }

    public bool IsDirty() {
        return _events.Any();
    }

    public IReadOnlyCollection<IEntityEvent> GetUncommittedEvents() {
        return _events.ToArray();
    }

    public void ClearUncommitted() {
        ClearErrors();
        ClearEvents();
    }

    public IReadOnlyCollection<IEntityError> GetUncommittedErrors() {
        return _errors.ToArray();
    }

    protected virtual void Raise(IEntityUpdated @event) {
        _events.Add(@event);
    }

    protected virtual void Raise(IEntityCreated @event) {
        if (_events.OfType<IEntityCreated>()
            .Any()) {
            throw new InvalidOperationException("The object is already created");
        }

        _events.Add(@event);
    }

    protected virtual void Raise(IEntityDeleted @event) {
        if (_events.OfType<IEntityDeleted>()
            .Any()) {
            throw new InvalidOperationException("The object is already deleted");
        }

        _events.Add(@event);
    }

    protected void Raise(IEntityError error) {
        _errors.Add(error);
    }

    public void ClearEvents() {
        _events.Clear();
    }

    public void ClearErrors() {
        _errors.Clear();
    }
}