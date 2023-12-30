namespace DropWord.Domain.Common;

public abstract class BaseAuditableEntity<T> : BaseEntity<T>,IBaseAuditableEntity where T : struct
{
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? WhenDeleted { get; set; }
}

public interface IBaseAuditableEntity
{
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? WhenDeleted { get; set; }
}