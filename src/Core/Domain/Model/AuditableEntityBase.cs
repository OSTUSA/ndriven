using System;

namespace Core.Domain.Model
{
    public enum AuditStatuses : int
    {
        None = 0,
        Active = 1,
        Archived = 2,
        Deleted = 3
    }

    abstract public class AuditableEntityBase<T> : EntityBase<T>, IAuditableEntity<T> where T : IAuditableEntity<T>, IEntity<T>
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime ArchivedOn { get; set; }
        public string ArchivedBy { get; set; }

        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        #region HELPER PROPERTIES

        public bool IsActive()
        {
            return !IsArchived() && !IsDeleted();
        }

        public bool IsUpdated()
        {
            return !UpdatedOn.Equals(default(DateTime)) && !UpdatedBy.Equals(string.Empty);
        }

        public bool IsArchived()
        {
            return !ArchivedOn.Equals(default(DateTime)) && !ArchivedBy.Equals(string.Empty);
        }

        public bool IsDeleted()
        {
            return !DeletedOn.Equals(default(DateTime)) && !DeletedBy.Equals(string.Empty);
        }

        public AuditStatuses AuditStatus
        {
            get
            {
                var status = AuditStatuses.None;

                if (IsActive()) status = AuditStatuses.Active;
                if (IsArchived()) status = AuditStatuses.Archived;
                if (IsDeleted()) status = AuditStatuses.Deleted;

                return status;
            }
        }

        #endregion
    }
}
