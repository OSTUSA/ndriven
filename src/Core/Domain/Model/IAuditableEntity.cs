using System;

namespace Core.Domain.Model
{
    public interface IAuditableEntity<T> : IAuditableEntity where T : IAuditableEntity<T>
    {

    }

    public interface IAuditableEntity
    {
        bool IsUpdated();
        bool IsArchived();
        bool IsDeleted();

        DateTime CreatedOn { get; set; }
        string CreatedBy { get; set; }

        DateTime UpdatedOn { get; set; }
        string UpdatedBy { get; set; }

        DateTime ArchivedOn { get; set; }
        string ArchivedBy { get; set; }

        DateTime DeletedOn { get; set; }
        string DeletedBy { get; set; }
    }
}
