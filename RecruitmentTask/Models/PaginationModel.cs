namespace RecruitmentTask.Models
{
    public enum SortDirection
    {
        ASC,
        DESC
    }

    public class PaginationModel<T>
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public List<T> Items { get; }

        public PaginationModel(List<T> items, int pageIndex, int pageSize, string sortField = null, SortDirection sortDirection = SortDirection.ASC)
        {
            if (!string.IsNullOrEmpty(sortField))
            {
                if (typeof(T).GetProperty(sortField) == null)
                {
                    throw new ArgumentException("Sort field does not exist.", nameof(sortField));
                }

                if (sortDirection == SortDirection.ASC)
                {
                    items = items.OrderBy(x => typeof(T).GetProperty(sortField).GetValue(x, null)).ToList();
                }
                else
                {
                    items = items.OrderByDescending(x => typeof(T).GetProperty(sortField).GetValue(x, null)).ToList();
                }
            }

            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(items.Count / (double)pageSize);

            Items = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static PaginationModel<T> Create(List<T> items, int pageIndex, int pageSize, string sortField = null, SortDirection sortDirection = SortDirection.ASC)
        {
            return new PaginationModel<T>(items, pageIndex, pageSize, sortField, sortDirection);
        }

    }
}
