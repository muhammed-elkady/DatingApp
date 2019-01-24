using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Core.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }


        private PagedList(List<T> items, int count, int currentPageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = currentPageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }


        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int currentPageNumber, int pageSize)
        {
            if (source != null)
            {
                var count = source.CountAsync();
                var items = source.Skip((currentPageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PagedList<T>(await items, await count, currentPageNumber, pageSize);
            }
            throw new ArgumentNullException();
        }
    }
}
