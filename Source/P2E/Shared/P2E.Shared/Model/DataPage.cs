using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Shared.Model
{
    public class DataPage<T>
    {
        public DataPage()
        {
            Items = new List<T>();
            CurrentPage = 1;
            PageSize = 10;

            // calcula total de páginas
            TotalPages = TotalItems > 0 ? ((int)Math.Ceiling((decimal)TotalItems / (decimal)PageSize)) : 1;

            // ensure current page isn't out of range
            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
            else if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }
        }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public string OrderBy { get; set; }
        public bool Descending { get; set; }

        public IList<T> Items { get; set; }

        public int TotalItems => (int)Items?.Count;

        public int TotalPages { get; set; }

        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public IEnumerable<int> Pages { get; private set; }
    }
}
