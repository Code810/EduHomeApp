using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.ViewModels
{

    public class PaginationVm<T> : List<T>
    {
        public int CurrentPage { get; }
        public int TotalPage { get; }
        public bool HasNext => CurrentPage < TotalPage;
        public bool HasPrev => CurrentPage > 1;
        public int Start { get; set; }
        public int End { get; set; }
        public PaginationVm(List<T> item, int currentPage, int totalPage)
        {
            CurrentPage = currentPage;
            TotalPage = totalPage;
            this.AddRange(item);
            int start = currentPage - 2;
            int end = currentPage + 1;
            if (start <= 0)
            {
                //end = end - (start - 1);
                start = 1;
            }
            if (end >= totalPage)
            {
                end = totalPage;
                //start = totalPage - 4;
            }
            Start = start;
            End = end;
        }

        public static async Task<PaginationVm<T>> Create(IQueryable<T> query, int page, int take)
        {
            if (page <= 0)
            {
                page = 1;
            }
            var data = await query
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();
            var totalPage = (int)Math.Ceiling((decimal)query.Count() / take);
            if (page > totalPage)
            {
                page = totalPage;
            }
            return new PaginationVm<T>(data, page, totalPage);
        }
    }
}
