using System;
using System.Linq.Expressions;


namespace Core.Domain.Model.Pagination
{
    public class PageParams<T> where T : IEntity<T>
    {
        public PageParams()
        {
            Page = 1;
            Count = 10;
            SortColumn = string.Empty;
            SortOrder = string.Empty;
            SearchPhrase = string.Empty;
            CustomFilter = string.Empty;
            ShowArchived = true;
            ShowDeleted = false;
        }

        public int Page { get; set; }
        public int Count { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string SearchPhrase { get; set; }
        public string CustomFilter { get; set; }
        public Expression<Func<T, bool>> SearchPredicate { get; set; }
        public bool ShowArchived { get; set; }
        public bool ShowDeleted { get; set; }

        public string DynamicOrderBy
        {
            get
            {
                // We need to force the existance of the sort colum and order or the linq query will blow up
                if (string.IsNullOrWhiteSpace(SortColumn))
                    throw new ApplicationException("SortColumn must be set and is missing!");

                if (string.IsNullOrWhiteSpace(SortOrder))
                    throw new ApplicationException("SortOrder must be set and is missing!");

                return string.Format("{0} {1}", SortColumn, SortOrder.ToUpper());
            }
        }
    }
}
