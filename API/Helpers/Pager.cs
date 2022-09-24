namespace API.Helpers
{
    public class Pager<T> where T : class
    {
        public string Search { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int Total { get; private set; }

        public IEnumerable<T> Registers { get; private set; }

        public Pager(IEnumerable<T> regiters, int total, int pageIndex, int pageSize, string search)
        {
            Registers = regiters;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Search = search;
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
