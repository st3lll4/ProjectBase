using Base.Contracts;

namespace Base.Domain;

public abstract class BaseUserEntity <TUser> :  BaseUserEntity<Guid, TUser>, IDomainId, IDomainUserId
    where TUser : class
{
    
}

public abstract class BaseUserEntity<TKey, TUser> : BaseEntity<TKey>, IDomainUserId<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class
{
    public TKey UserId { get; set; } = default!;
    public TUser? User { get; set; }

}