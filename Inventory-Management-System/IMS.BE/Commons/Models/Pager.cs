namespace IMS.BE.Commons.Models
{
    public class Pager
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPages { get; set; }
        public int EndPages { get; set; }

        public Pager(int totalPages, int page, int pageSize = 10)
        {

            int currentPage = page;
            //hal 1
            int startPage = currentPage - 5;//-4
            int endPage = currentPage + 4;//5
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPages = startPage;
            EndPages = endPage;
        }
    }
}
