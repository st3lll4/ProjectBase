using Base.Contracts;

namespace Base.Domain;

public abstract class BaseEntityId : IDomainId
{
    public Guid Id { get; set; }
}