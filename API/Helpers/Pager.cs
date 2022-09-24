namespace API.Helpers
{
    public class Pager<T> where T : class
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public IEnumerable<T> Registers { get; set; }

        public Pager(IEnumerable<T> regiters, int total, int pageIndex, int pageSize)
        {
            Registers = regiters;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(Total / (double)PageSize);
            }
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
    }
}
