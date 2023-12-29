namespace E_CommerceWebApp.ViewModel
{
    public class PagedViewModel<T>
    {
        public T Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public string? SearchQuery { get; set; }

        public PagedViewModel(T data, int pageNumber, int pageSize, int totalRecords, string searchQuery)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            LastPage = (int)Math.Ceiling((double)totalRecords / PageSize);
            TotalPages = totalRecords;
            TotalRecords = totalRecords;
            SearchQuery = searchQuery;
        }
    }
}
